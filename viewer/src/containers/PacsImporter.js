import React, { Component } from "react";
import { Table } from "react-bootstrap";

export default class PacsImporter extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            patients: []
        };
    }
    componentDidMount() {
        this.getPatients();
    }

    getPatientDetails() {
        const patientId = this.props.match.params.patientId;
        const that = this;
        fetch(`http://localhost:8042/patiens/`)
            .then(function (response) {
                console.log(response);
                return response.json();
            })
            .then(jsonData => {
                this.setState({ patients: jsonData });

            })
    }

    importPatient(patientId) {
        fetch(`http://localhost:8042/patients/${patientId}/anonymize`)
        .then(function (response) {
            console.log(response);
            return response.json();
        })
        .then(jsonData => {
            fetch(`http://localhost:8042/patients/${patientId}/archive`)
            .then(function (response) {
                console.log(response);
                return response.json();
            })
            .then(jsonData => {
                fetch(`http://localhost:5001/api/NewDicom/Upload`)
                .then(function (response) {
                    console.log(response);
                    return response.json();
                })
                .then(jsonData => {
                    console.log("Patient Uploaded");
                })
            })       
        })
    }

    render() {
        const listItems = this.state.patients.map((d, index) => <tr><td>index</td><td>d.patient.name</td><td><button onClick={() => this.importPatient(d.id)}className="btn btn-primary">Import</button></td></tr>);
        return (
            <Table striped bordered condensed hover>
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Patient Name</th>
                        <th>Action</th>
                    </tr>
                </thead>
                {}
                <tbody>
                    {listItems}
                </tbody>
            </Table>
        );
    }
}
