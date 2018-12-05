import React, { Component } from "react";
import axios from 'axios';
import Button from 'react-bootstrap-button-loader';
import FileBase64 from 'react-file-base64';

import "./FileUpload.css";

export default class FileUpload extends Component {

    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            uploadStatus: false,
            fileName: null,
            files: null
        }
        this.handleUploadImage = this.handleUploadImage.bind(this);
    }

    handleUploadImage(ev) {
        ev.preventDefault();
        this.setState({'loading': true});

        let that = this;
        let files_without_type = this.state.files;
        axios.post('http://localhost:5001/api/NewDicom/UploadList', files_without_type)
        .then(function (response) {
            that.setState({uploadStatus: true, loading: false, fileName: "File" });
        })
        .catch(function (error) {
            console.log(error);
        });
 
    }

    getFiles(files) {
        let files_without_type = [];
        for (let file of files) {
            files_without_type.push({'base64Dicom': file.base64.replace('data:application/dicom;base64,', '').replace('data:application/octet-stream;base64,','')});
        }
        this.setState({files: files_without_type})

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
                    <FileBase64
                    className="form-control"
                    multiple={ true }
                    onDone={ this.getFiles.bind(this) } />
                </div>

                {!this.state.uploadStatus ?
                    <Button loading={this.state.loading} className="btn btn-success" onClick={this.handleUploadImage} type='button'>{this.state.uploadStatus ? 'Done' : 'Upload' }</Button>
                    : null}
                {this.state.uploadStatus ? <p>Patient {this.state.fileName} has been added.</p>: null}

            </div>
        );
    }
}