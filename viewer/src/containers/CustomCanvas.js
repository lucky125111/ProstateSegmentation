import React, { Component, PropTypes } from 'react';
import { findDOMNode } from 'react-dom'

import { Pencil, TOOL_PENCIL, Rubber, TOOL_RUBBER } from './tools'

export const toolsMap = {
    [TOOL_PENCIL]: Pencil,
    [TOOL_RUBBER]: Rubber
};

export default class CustomCanvas extends Component {

    tool = null;
    interval = null;

    // static propTypes = {
    //   width: PropTypes.number,
    //   height: PropTypes.number,
    //   items: PropTypes.array.isRequired,
    //   animate: PropTypes.bool,
    //   canvasClassName: PropTypes.string,
    //   color: PropTypes.string,
    //   fillColor: PropTypes.string,
    //   size: PropTypes.number,
    //   tool: PropTypes.string,
    //   toolsMap: PropTypes.object,
    //   onItemStart: PropTypes.func, // function(stroke:Stroke) { ... }
    //   onEveryItemChange: PropTypes.func, // function(idStroke:string, x:number, y:number) { ... }
    //   onDebouncedItemChange: PropTypes.func, // function(idStroke, points:Point[]) { ... }
    //   onCompleteItem: PropTypes.func, // function(stroke:Stroke) { ... }
    //   debounceTime: PropTypes.number,
    // };

    static defaultProps = {
        width: 500,
        height: 500,
        color: '#000',
        size: 5,
        fillColor: '',
        canvasClassName: 'canvas',
        debounceTime: 1000,
        animate: true,
        tool: TOOL_PENCIL,
        toolsMap
    };

    constructor(props) {
        super(props);
        this.initTool = this.initTool.bind(this);
        this.onMouseDown = this.onMouseDown.bind(this);
        this.onMouseMove = this.onMouseMove.bind(this);
        this.onDebouncedMove = this.onDebouncedMove.bind(this);
        this.onMouseUp = this.onMouseUp.bind(this);
    }

    componentDidMount() {
        this.backgroundCanvas = findDOMNode(this.backgroundCanvasRef);
        this.backgroundCtx = this.backgroundCanvas.getContext('2d');
        this.canvas = findDOMNode(this.foregroundCanvasRef);
        this.ctx = this.canvas.getContext('2d');
        var foregroundCtx = this.ctx;
        var local_canvas = this.canvas;

        var backgroundImage = new Image;

        var backgroundCtx = this.backgroundCtx;
        var backgroudWidth = 0;
        var backgroundHeight = 0;

        backgroundImage.onload = function () {
            backgroundCtx.canvas.width = this.width;
            backgroundCtx.canvas.height = this.height;
            local_canvas.width = this.width;
            local_canvas.height = this.height;
            backgroundCtx.drawImage(backgroundImage, 0, 0, this.width, this.height);
        };
        backgroundImage.src = this.props.backgroundImageUrl;


        this.initTool(this.props.tool);

        var foregroundImage = new Image;

        foregroundImage.onload = function () {
            foregroundCtx.drawImage(foregroundImage, 0, 0, this.width, this.height);
        };
        foregroundImage.src = this.props.foregroundImageUrl;
    }

    componentWillReceiveProps({ tool, items }) {
        items
            .filter(item => this.props.items.indexOf(item) === -1)
            .forEach(item => {
                this.initTool(item.tool);
                this.tool.draw(item, this.props.animate);
            });
        this.initTool(tool);
    }

    initTool(tool) {
        this.tool = this.props.toolsMap[tool](this.ctx);
    }

    onMouseDown(e) {
        const data = this.tool.onMouseDown(...this.getCursorPosition(e), this.props.color, this.props.size, this.props.fillColor);
        data && data[0] && this.props.onItemStart && this.props.onItemStart.apply(null, data);
        if (this.props.onDebouncedItemChange) {
            this.interval = setInterval(this.onDebouncedMove, this.props.debounceTime);
        }
    }

    onDebouncedMove() {
        if (typeof this.tool.onDebouncedMouseMove == 'function' && this.props.onDebouncedItemChange) {
            this.props.onDebouncedItemChange.apply(null, this.tool.onDebouncedMouseMove());
        }
    }

    onMouseMove(e) {
        const data = this.tool.onMouseMove(...this.getCursorPosition(e));
        data && data[0] && this.props.onEveryItemChange && this.props.onEveryItemChange.apply(null, data);
    }

    onMouseUp(e) {
        const data = this.tool.onMouseUp(...this.getCursorPosition(e));
        data && data[0] && this.props.onCompleteItem && this.props.onCompleteItem.apply(null, data);
        if (this.props.onDebouncedItemChange) {
            clearInterval(this.interval);
            this.interval = null;
        }
    }

    getCursorPosition(e) {
        const { top, left } = this.canvas.getBoundingClientRect();
        return [
            e.clientX - left,
            e.clientY - top
        ];
    }

    render() {
        const { width, height, canvasClassName } = this.props;
        return (
            <div style={{ clear: "both", position: "relative", margin: "0 auto", width: "50%" }}>
                <canvas
                    style={{ zIndex: 0, position: "absolute", left: 0, top: 0 }}
                    ref={(canvas) => { this.backgroundCanvasRef = canvas; }}
                    className={canvasClassName}
                    onMouseDown={this.onMouseDown}
                    onMouseMove={this.onMouseMove}
                    onMouseOut={this.onMouseUp}
                    onMouseUp={this.onMouseUp}
                />
                <canvas
                    style={{ zIndex: 1, position: "absolute", left: 0, top: 0 }}
                    ref={(canvas) => { this.foregroundCanvasRef = canvas; }}
                    className={canvasClassName}
                    onMouseDown={this.onMouseDown}
                    onMouseMove={this.onMouseMove}
                    onMouseOut={this.onMouseUp}
                    onMouseUp={this.onMouseUp}

                />
            </div>

        )
    }


}