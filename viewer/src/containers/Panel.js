import React, { Component } from "react";
import { Modal } from "react-bootstrap";
import { Link } from "react-router-dom";
import FileUpload from './FileUpload';
import 'filepond/dist/filepond.min.css';
import "./Panel.css";

export default class Panel extends Component {
    constructor(props) {
        super(props);
        this.handleShow = this.handleShow.bind(this);
        this.handleClose = this.handleClose.bind(this);
        this.state = {
            patients: [],
            show: false
        };
    }

    handleClose() {
        this.setState({ show: false });
    }

    handleShow() {
        this.setState({ show: true });
    }

    componentDidMount() {
        this.getPatients();
    }

    getPatients() {
        const that = this;
        fetch("http://localhost:5001/api/Patient")
            .then(function (response) {
                return response.json();
            })
            .then(jsonData => {
                this.setState({ patients: jsonData });
            })
        // fetch('http://localhost:8000/patients')
        //     .then(({ results }) => this.setState({ patients: results }));
    }

    render() {
        if (this.state.patients.length > 0) {

        }
        const patients = this.state.patients.map((patient, i) => (
            <div key={patient.id} className="col-sm-3">
                <div className="card card-default">
                    <Link to={"/patient/" + patient.id} className="card-body">
                        <p className="lead">{patient.name}</p>
                    </Link>
                </div>
            </div>

        ));

        return (
            <div>
                <Modal show={this.state.show} onHide={this.handleClose}>
                    <Modal.Header closeButton>
                        <Modal.Title>Modal heading</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <FileUpload />

                    </Modal.Body>
                </Modal>
                <div className="clearfix">
                    <button className="btn btn-success pull-right" onClick={this.handleShow} ><i className="glyphicon glyphicon-plus"></i>Add new patient</button>
                </div>
                <hr />
                <div id="layout-content" className="layout-content-wrapper">
                    <div className="row">{patients}</div>
                </div>
            </div>

        );
    }
}
