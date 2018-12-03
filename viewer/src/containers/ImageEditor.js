import React, { Component } from "react";
import CustomCanvas from "./CustomCanvas";
import { Breadcrumb } from "react-bootstrap";
import { TOOL_PENCIL, TOOL_RUBBER } from "./tools";
import { Link } from "react-router-dom";

import "./ImageEditor.css";

export default class ImageEditor extends Component {
    constructor(props) {
        super(props);

        this.state = {
            tool: TOOL_PENCIL,
            size: 2,
            color: 'rgba(255,0,0,0.5)',
            items: [],
            patientData: null,
            sliceUrl: null,
            maskUrl: null,
            currentImgIndex: 0,
            indices: []
        }
        this.moveRight = this.moveRight.bind(this);
        this.moveLeft = this.moveLeft.bind(this);
    }

    componentDidMount() {
        this.getPatientDetails();
    }

    getPatientDetails() {
        const patientId = this.props.match.params.patientId;
        const that = this;
        fetch(`http://localhost:5001/api/PatientData/?Id=${patientId}`)
            .then(function (response) {
                console.log(response);
                return response.json();
            })
            .then(jsonData => {
                this.setState({ patientData: jsonData });
                console.log(jsonData);
            })


        // fetch(`http://localhost:5001/api/DicomSlice/?PatientId.Id=${patienId}&SliceIndex=0`)
        //     .then(function (response) {
        //         console.log(response);
        //         return response.json();
        //     })
        //     .then(jsonData => {
        //         this.setState({ maskUrl: 'data:image/png;base64,' + jsonData['image'] });
        //         console.log(jsonData);
        //     })
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

    loadSliceAndMask(sliceId) {
        const patientId = this.props.match.params.patientId;

        // Load Slice
        fetch(`http://localhost:5001/api/Slice/${patientId}&${sliceId}`)
            .then(function (response) {
                return response.json();
            })
            .then(jsonData => {
                this.setState({ sliceUrl: 'data:image/png;base64,' + jsonData['image'] });
                console.log(jsonData);
            })
        // Load Mask
        fetch(`http://localhost:5001/api/Mask/${patientId}&${sliceId}`)
            .then(function (response) {
                return response.json();
            })
            .then(jsonData => {
                console.log("This mask?");
                console.log(jsonData);
                this.setState({ maskUrl: 'data:image/png;base64,' + jsonData['mask'] });

            })


    }

    moveRight() {
        const newCurrentImgIndex = this.state.indices[this.state.indices.indexOf(this.state.currentImgIndex) + 1];
        this.loadSliceAndMask(newCurrentImgIndex);
        this.setState({'currentImgIndex': newCurrentImgIndex});
    }

    moveLeft() {
        const newCurrentImgIndex = this.state.indices[this.state.indices.indexOf(this.state.currentImgIndex) - 1];
        this.loadSliceAndMask(newCurrentImgIndex);
        this.setState({'currentImgIndex': newCurrentImgIndex});
    }

    render() {
        const dicomId = this.props.match.params.patientId;
        const sliceId = this.state.currentImgIndex;
        const { tool, size, color, items } = this.state;
        console.log("SSTATE");
        console.log(this.state);
        const leftArrow = this.state.indices && this.state.currentImgIndex === this.state.indices[0] ? null : (
            <div className="pull-left">
                <a onClick={this.moveLeft}><i className="glyphicon glyphicon-chevron-left"></i></a>
            </div>
        );
        const rightArrow = this.state.indices && this.state.currentImgIndex === this.state.indices[this.state.indices.length-1] ? null : (
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

                <div className="lander" style={{ margin: "0 auto", width: "50%" }}>

                    <div style={{ margin: "0 auto", width: "50%" }}>
                        <div className="tools" style={{ marginBottom: 20 }}>
                            <button
                                style={tool === TOOL_PENCIL ? { fontWeight: 'bold' } : undefined}
                                className={tool === TOOL_PENCIL ? 'item-active' : 'item'}
                                onClick={() => this.setState({ tool: TOOL_PENCIL, color: "rgba(255,0,0,0.5)" })}
                            >Pencil</button>
                            <button
                                style={tool === TOOL_RUBBER ? { fontWeight: 'bold' } : undefined}
                                className={tool === TOOL_RUBBER ? 'item-active' : 'item'}
                                onClick={() => this.setState({ tool: TOOL_RUBBER, color: "rgba(0,0,0,1)" })}
                            >Rubber</button>
                        </div>
                        <div className="options" style={{ marginBottom: 20 }}>
                            <label htmlFor="">Size: </label>
                            <input min="1" max="20" type="range" value={size} onChange={(e) => this.setState({ size: parseInt(e.target.value) })} />
                        </div>
                    </div>

                    <CustomCanvas dicomId={dicomId} sliceId={sliceId} items={items} size={size} tool={tool} color={color} backgroundImageUrl={this.state.sliceUrl} foregroundImageUrl={this.state.maskUrl} />
                    {leftArrow}
                    {rightArrow}
                </div>

            </div>
        );
    }
}