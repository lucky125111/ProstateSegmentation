import React, { Component } from "react";
import CustomCanvas from "./CustomCanvas";
import { TOOL_PENCIL, TOOL_RUBBER } from "./tools";

import "./ImageEditor.css";

export default class ImageEditor extends Component {
    constructor(props) {
        super(props);

        this.state = {
            tool: TOOL_PENCIL,
            size: 2,
            color: 'rgba(255,0,0,0.5)',
            fill: false,
            fillColor: '#444444',
            items: []
        }
    }

    render() {
        const { tool, size, color, fill, fillColor, items } = this.state;
        return (
            <div className="Home">
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
                            <label htmlFor="">size: </label>
                            <input min="1" max="20" type="range" value={size} onChange={(e) => this.setState({ size: parseInt(e.target.value) })} />
                        </div>x
                    </div>
                    <CustomCanvas items={items} size={size} tool={tool} color={color} backgroundImageUrl={"http://localhost:8000/static/example_image.jpg"} foregroundImageUrl={"http://localhost:8000/static/example_mask.png"} />

                </div>
            </div>
        );
    }
}