import React, { Component } from "react";
import { Modal } from "react-bootstrap";
import { Link } from "react-router-dom";
import FileUpload from './FileUpload';
import PacsImporter from './PacsImporter';
import 'filepond/dist/filepond.min.css';
import "./Panel.css";

export default class Panel extends Component {
    constructor(props) {
        super(props);
        this.handleShowUpload = this.handleShowUpload.bind(this);
        this.handleShowImport = this.handleShowImport.bind(this);
        this.handleClose = this.handleClose.bind(this);
        this.state = {
            patients: [],
            show: false,
            type: ''
        };
    }

    handleClose() {
        this.setState({ show: false });
    }

    handleShowUpload() {
        this.setState({ show: true, type: 'upload'});
    }

    handleShowImport() {
        this.setState({ show: true, type: 'import'});

    }

    componentDidMount() {
        this.getPatients();
    }

    getPatients() {
        const that = this;
        fetch("http://localhost:5001/api/PatientData")
            .then(function (response) {
                return response.json();
            })
            .then(jsonData => {
                this.setState({ patients: jsonData });
                console.log(jsonData);
            })
        // fetch('http://localhost:8000/patients')
        //     .then(({ results }) => this.setState({ patients: results }));
    }

    render() {
        if (this.state.patients.length > 0) {

        }
        const patients = this.state.patients.map((patient, i) => (
            <div key={patient.dicomModelId} className="col-sm-3">
                <div className="card card-default">
                    <Link to={"/patient/" + patient.dicomModelId} className="card-body">
                        <p className="lead">{patient.patientName}</p>
                    </Link>
                </div>
            </div>

        ));
        const ModalContent = this.state.type === 'upload' ? <FileUpload/> : <PacsImporter/>;

        return (
            <div>
                <Modal show={this.state.show} onHide={this.handleClose}>
                    <Modal.Header closeButton>
                        <Modal.Title>{this.state.type === 'upload' ? "Upload Patient": "Import Patient" }</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        {ModalContent}

                    </Modal.Body>
                </Modal>
                <div className="clearfix">
                    <button className="btn btn-success pull-right" onClick={this.handleShowUpload} ><i className="glyphicon glyphicon-plus"></i>Add new patient</button>
                    <button className="btn btn-primary pull-right" onClick={this.handleShowImport} ><i className="glyphicon glyphicon-plus"></i>Import patient from PACS</button>

                    </div>
                <hr />
                <div id="layout-content" className="layout-content-wrapper">
                    <div className="row">{patients}</div>
                </div>
            </div>

        );
    }
}
