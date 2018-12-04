from io import BytesIO

import base64

import numpy as np
from flask import (
    Flask,
    request,
    jsonify
)

from helpers import predict_mask, convert_base64_to_image


app = Flask(__name__)


@app.route('/predict_mask/', methods=['POST'])
def predict_base64_mask():
    if request.method == 'POST':
        base64_image = request.get_json()['image']
        np_image = np.array(convert_base64_to_image(base64_image))
        base64_predicted_mask = predict_mask(np_image)
        print(base64_predicted_mask)
        return jsonify({'mask': base64_predicted_mask})

if __name__ == "__main__":
    app.run(host="0.0.0.0", debug=True)


