import React, { Component } from "react";
import axios from 'axios';
import Button from 'react-bootstrap-button-loader';

import "./FileUpload.css";

export default class FileUpload extends Component {

    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            uploadStatus: false,
            fileName: null
        }
        this.handleUploadImage = this.handleUploadImage.bind(this);
    }


    handleUploadImage(ev) {
        ev.preventDefault();
        this.setState({'loading': true});

        const data = new FormData();
        let dicom_file = this.uploadInput.files[0];
        data.append('file', dicom_file);
        var that = this;
        let base64file = '';
        this.getBase64(dicom_file, (result) => {
            base64file = result.replace('data:application/dicom;base64,', '');
            axios.post('http://localhost:5001/api/NewDicom', {'base64Dicom': base64file})
            .then(function (response) {
                that.setState({uploadStatus: true, loading: false, fileName: dicom_file.name });
            })
            .catch(function (error) {
                console.log(error);
            });
        });
 
    }

    getBase64(file, cb) {
        let reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function () {
            cb(reader.result)
        };
        reader.onerror = function (error) {
            console.log('Error: ', error);
        };
    }

    render() {
        return (
            <div className="container">
                <div className="form-group">
                    <input className="form-control" ref={(ref) => { this.uploadInput = ref; }} type="file" />
                </div>

                <Button loading={this.state.loading} className="btn btn-success" onClick={this.handleUploadImage} type='button'>{this.state.uploadStatus ? 'Done' : 'Upload' }</Button>
                {this.state.uploadStatus ? <p>Patient {this.state.fileName} has been added.</p>: null}

            </div>
        );
    }
}