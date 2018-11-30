import base64
from io import BytesIO

import numpy as np
from PIL import Image
import PIL.ImageOps

import scipy.ndimage
from keras.optimizers import Adam

from skimage.exposure import equalize_hist
from skimage.transform import resize

from metrics import dice_coef, dice_coef_loss, numpy_dice
from models import actual_unet, simple_unet


IMG_SIZE = 400
IMG_ROWS = 96
IMG_COLS = 96
WEIGHTS_FILE = 'weights.h5'


def convert_base64_to_image(data):
    return Image.open(BytesIO(base64.b64decode(data)))


def convert_image_to_base64(image):
    buffered = BytesIO()
    image.save(buffered, format="PNG")
    return base64.b64encode(buffered.getvalue()).decode('ascii')


def normalize_np_image(np_image, size=IMG_SIZE):
    img = equalize_hist(np_image)
    img = resize(img, (IMG_ROWS, IMG_COLS), preserve_range=True)
    return img


def inverse_colors(image):
    r, g, b, a = image.split()
    rgb_image = Image.merge('RGB', (r, g, b))
    inverted_image = PIL.ImageOps.invert(rgb_image)

    r2, g2, b2 = inverted_image.split()
    final_transparent_image = Image.merge('RGBA', (r2, g2, b2, a))
    return final_transparent_image


def convert_np_mask_to_image(np_predicted_mask, width, height):
    squeezed = np.squeeze(np_predicted_mask, axis=0)
    squeezed = np.squeeze(squeezed, axis=2)
    img = Image.fromarray(squeezed).convert('RGBA')
    img = inverse_colors(img)
    img = img.resize((width, height))

    pixel_data = img.load()
    width, height = img.size

    for y in range(height):
        for x in range(width):
            if pixel_data[x, y] == (255, 255, 255, 255):
                pixel_data[x, y] = (0, 0, 0, 0)
            else:
                pixel_data[x, y] = (255, 0, 0, 127)

    return img


def predict_mask(np_image):
    # print(np_image)
    width, height, channels = np_image.shape
    normalized_img = normalize_np_image(np_image)

    # Expanding dims to match expected by model
    new_img = np.expand_dims(normalized_img, axis=0)
    new_img = np.expand_dims(new_img, axis=3)

    model = simple_unet(IMG_ROWS, IMG_COLS)
    model.load_weights(WEIGHTS_FILE)
    model.compile(optimizer=Adam(), loss=dice_coef_loss, metrics=[dice_coef, 'binary_accuracy'])
    np_predicted_mask = model.predict(new_img, verbose=1)

    return convert_image_to_base64(convert_np_mask_to_image(np_predicted_mask, width, height))
