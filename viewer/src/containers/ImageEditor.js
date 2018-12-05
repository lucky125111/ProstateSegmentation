import React, { Component } from "react";
import CustomCanvas from "./CustomCanvas";
import { Breadcrumb } from "react-bootstrap";
import { TOOL_PENCIL, TOOL_RUBBER } from "./tools";
import { Link } from "react-router-dom";
import Toggle from 'react-bootstrap-toggle';

import "./ImageEditor.css";
import axios from "axios/index";

export default class ImageEditor extends Component {
    constructor(props) {
        super(props);

        this.state = {
            tool: TOOL_PENCIL,
            size: 10,
            color: 'rgba(255,0,0,0.5)',
            items: [],
            patientData: null,
            sliceUrl: null,
            maskUrl: null,
            currentImgIndex: 0,
            indices: [],
            toggleActive: true,
            volume: 0
        }
        this.moveRight = this.moveRight.bind(this);
        this.moveLeft = this.moveLeft.bind(this);
        this.onToggle = this.onToggle.bind(this);
    }

    componentDidMount() {
        this.getPatientDetails();
    }

    getPatientDetails() {
        const patientId = this.props.match.params.patientId;
        const that = this;
        fetch(`http://localhost:5001/api/PatientData/${patientId}`)
            .then(function (response) {
                console.log(response);
                return response.json();
            })
            .then(jsonData => {
                this.setState({ patientData: jsonData });
                console.log("Patient Data");
                console.log(jsonData);
            })
        fetch(`http://localhost:5001/api/Dicom/${patientId}`)
            .then(function (response) {
                console.log(response);
                return response.json();
            })
            .then(jsonData => {
                this.setState({ dicomData: jsonData });
                console.log("Dicom Data");
                console.log(jsonData);
            })

        fetch(`http://localhost:5001/api/Dicom/Indexes/${patientId}`)
            .then(function (response) {
                console.log(response);
                return response.json();
            })
            .then(jsonData => {
                console.log(jsonData);
                this.setState({ indices: jsonData, currentImgIndex: jsonData[0] });
                this.loadSliceAndMask(jsonData[0]);
            })
    }

    calculatePatientVolume(type) {
        const patientId = this.props.match.params.patientId;
        const that = this;

        axios.post(`http://localhost:5001/api/Volume/Calculate/${patientId}&${type}`)
            .then(function (response) {
                console.log(response);
            })
            .catch(function (error) {
                console.log(error);
            });

        fetch(`http://localhost:5001/api/Volume/${patientId}`)
            .then(function (response) {
                console.log(response);
                return response.json();
            })
            .then(jsonData => {
                console.log(jsonData);
                this.setState({ volume: jsonData});
            })
    }

    loadSliceAndMask(sliceId) {
        const patientId = this.props.match.params.patientId;

        // Load Slice
        fetch(`http://localhost:5001/api/Slice/${patientId}&${sliceId}`)
            .then(function (response) {
                return response.json();
            })
            .then(jsonData => {
                this.setState({
                    sliceUrl: 'data:image/png;base64,' + jsonData['image'],
                    maskUrl: 'data:image/png;base64,' + jsonData['mask']
                });
                console.log(jsonData);
            })
    }

    onToggle() {
        this.setState({ toggleActive: !this.state.toggleActive });
    }

    moveRight() {
        const newCurrentImgIndex = this.state.indices[this.state.indices.indexOf(this.state.currentImgIndex) + 1];
        this.loadSliceAndMask(newCurrentImgIndex);
        this.setState({ 'currentImgIndex': newCurrentImgIndex });
    }

    moveLeft() {
        const newCurrentImgIndex = this.state.indices[this.state.indices.indexOf(this.state.currentImgIndex) - 1];
        this.loadSliceAndMask(newCurrentImgIndex);
        this.setState({ 'currentImgIndex': newCurrentImgIndex });
    }

    render() {
        const dicomId = this.props.match.params.patientId;
        const sliceId = this.state.currentImgIndex;
        const { tool, size, color, items, toggleActive } = this.state;
        const leftArrow = this.state.indices && this.state.currentImgIndex === this.state.indices[0] ? null : (
            <div className="pull-left">
                <a onClick={this.moveLeft}><i className="glyphicon glyphicon-chevron-left"></i></a>
            </div>
        );
        const rightArrow = this.state.indices && this.state.currentImgIndex === this.state.indices[this.state.indices.length - 1] ? null : (
            <div className="pull-right">
                <a onClick={this.moveRight}><i className="glyphicon glyphicon-chevron-right"></i></a>
            </div>
        );

        return (
            <div className="Home">
                <Breadcrumb>
                    <Breadcrumb.Item componentClass={Link} href="/" to="/">
                        Patients List
                     </Breadcrumb.Item>
                    <Breadcrumb.Item active> {'Patient ' + this.props.match.params.patientId}</Breadcrumb.Item>
                </Breadcrumb>
                <div className="col-lg-4 content-border">
                    <div className="panel panel-primary">
                        <div className="panel-heading">
                            <p>Patient Data</p>
                        </div>
                        <div className="panel-body">
                            <p>Patient Name: {this.state.patientData ? this.state.patientData.patientName : null}</p>
                            <p>Patient Age: {this.state.patientData ? this.state.patientData.patientAge : null}</p>
                            <p>Patient Sex: {this.state.patientData ? this.state.patientData.patientSex : null}</p>
                            <p>Patient Height: {this.state.patientData ? this.state.patientData.patientSize : null}</p>
                            <p>Patient Weight: {this.state.patientData ? this.state.patientData.patientWeight : null}</p>
                        </div>

                    </div>
                    <div className="panel panel-info">
                    <div className="panel-heading">
                        <p>Slice Data</p>
                    </div>
                    <div className="panel-body">
                        <p>Image height: {this.state.dicomData ? this.state.dicomData.imageHeight + "px" : null}</p>
                        <p>Image width: {this.state.dicomData ? this.state.dicomData.imageWidth + "px" : null}</p>
                        <p>Spacing between slices: {this.state.dicomData ? this.state.dicomData.spacingBetweenSlices : null}</p>
                        <p>Pixel spacing horizontal: {this.state.dicomData ? this.state.dicomData.pixelSpacingHorizontal : null}</p>
                        <p>Pixel spacing vertical: {this.state.dicomData ? this.state.dicomData.pixelSpacingVertical : null}</p>
                    </div>
                </div>
                </div>
                <div className="col-lg-8">
                    <div className="lander">
                    <div className="panel panel-primary">
                        <div className="panel-heading">
                            <p>Volume</p>
                        </div>
                        <div className="panel-body form-group">
                            <button style={{ marginRight: "10px", marginBottom: "10px" }} onClick={() => this.calculatePatientVolume("ConvexHull")} className="btn btn-primary">Calculate with Convex </button>
                            <button style={{ marginRight: "10px", marginBottom: "10px" }} onClick={() => this.calculatePatientVolume("Simple")} className="btn btn-primary">Calculate with Square </button>
                            <button style={{ marginRight: "10px", marginBottom: "10px" }} onClick={() => this.calculatePatientVolume("CountPixels")} className="btn btn-primary">Calculate with Biggest </button>
                            <input type="decimal" className="form-control" value={this.state.volume} disabled/>
                        </div>
                    </div>
                        <div style={{ margin: "auto", width: "50%" }}>
                            <div>
                                <Toggle
                                    style={{ marginBottom: 20 }}
                                    onClick={this.onToggle}
                                    offstyle="danger"
                                    on={"Show mask"}
                                    off={"Hide mask"}
                                    active={this.state.toggleActive}
                                />
                            </div>

                            <div className="tools" style={{ marginBottom: 20 }}>
                                <button
                                    style={tool === TOOL_PENCIL ? { fontWeight: 'bold' } : undefined}
                                    className={(tool === TOOL_PENCIL ? 'item-active' : 'item') + ' btn btn-default'}
                                    onClick={() => this.setState({ tool: TOOL_PENCIL, color: "rgba(255,0,0,0.5)" })}
                                ><i className="glyphicon glyphicon-pencil"/>Pencil</button>
                                <button
                                    style={tool === TOOL_RUBBER ? { fontWeight: 'bold' } : undefined}
                                    className={(tool === TOOL_RUBBER ? 'item-active' : 'item') + ' btn btn-default'}
                                    onClick={() => this.setState({ tool: TOOL_RUBBER, color: "rgba(0,0,0,1)" })}
                                ><i className="glyphicon glyphicon-erase"/>Rubber</button>
                            </div>

                            <div className="options" style={{ marginBottom: 20 }}>
                                <label htmlFor="">Size: </label>
                                <input min="1" max="20" type="range" value={size} onChange={(e) => this.setState({ size: parseInt(e.target.value) })} />
                            </div>
                        </div>

                        <CustomCanvas showMask={toggleActive} dicomId={dicomId} sliceId={sliceId} items={items} size={size} tool={tool} color={color} backgroundImageUrl={this.state.sliceUrl} foregroundImageUrl={this.state.maskUrl} />
                        {leftArrow}
                        {rightArrow}
                    </div>
                </div>
            </div>
        );
    }
}