import React, { Component } from "react";
import { Table } from "react-bootstrap";

export default class PacsImporter extends React.Component {
    render() {
        return (
            <Table striped bordered condensed hover>
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Patient Name</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>1</td>
                        <td>Prostate3T-03-0023</td>
                        <td><button className="btn btn-primary">Import</button></td>

                    </tr>
                    <tr>
                        <td>2</td>
                        <td>Prostate3T-03-0024</td>
                        <td><button className="btn btn-primary">Import</button></td>
                    </tr>
                    <tr>
                        <td>3</td>
                        <td>ProstateDx-01-0035</td>
                        <td><button className="btn btn-primary">Import</button></td>
                    </tr>
                </tbody>
            </Table>
        );
    }
}
