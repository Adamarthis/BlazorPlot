window.canvasRenderer = {
    clear: function (canvas) {
        if (!canvas) return;
        const ctx = canvas.getContext('2d');
        ctx.clearRect(0, 0, canvas.width, canvas.height);
    },

    drawAxes: function (canvas, centerX, centerY, uPP, width, height) {
        const ctx = canvas.getContext('2d');
        
        const visibleWidth = width * uPP;
        const rawStep = visibleWidth / 10;
        
        const magnitude = Math.pow(10, Math.floor(Math.log10(rawStep)));
        const norm = rawStep / magnitude;
        
        let majorStep;
        if (norm < 1.5) majorStep = 1 * magnitude;
        else if (norm < 3.5) majorStep = 2 * magnitude;
        else if (norm < 7.5) majorStep = 5 * magnitude;
        else majorStep = 10 * magnitude;

        const minorStep = majorStep / 5;

        const minX = centerX - (width / 2) * uPP;
        const maxX = centerX + (width / 2) * uPP;
        const minY = centerY - (height / 2) * uPP;
        const maxY = centerY + (height / 2) * uPP;

        ctx.font = '12px Segoe UI, sans-serif';
        ctx.fillStyle = '#000000';

        const xAxisY = (height / 2) + centerY / uPP;
        const yAxisX = (width / 2) - centerX / uPP;
        
        let textY = Math.max(5, Math.min(xAxisY + 5, height - 20));
        let textX = Math.max(30, Math.min(yAxisX - 8, width - 10));

        const formatLabel = (val) => parseFloat(val.toPrecision(12)).toString();

        ctx.textAlign = 'center';
        ctx.textBaseline = 'top';
        const startX = Math.floor(minX / minorStep) * minorStep;
        
        for (let x = startX; x <= maxX; x += minorStep) {
            const px = (width / 2) + (x - centerX) / uPP;
            const isMajor = Math.abs(Math.round(x / majorStep) * majorStep - x) < minorStep * 0.1;

            ctx.beginPath();
            ctx.strokeStyle = isMajor ? '#8c8c8c' : '#cccccc';
            ctx.lineWidth = isMajor ? 1.5 : 1;
            ctx.moveTo(px, 0);
            ctx.lineTo(px, height);
            ctx.stroke();

            if (isMajor && Math.abs(x) > 1e-9) {
                ctx.fillText(formatLabel(x), px, textY);
            }
        }

        ctx.textAlign = 'right';
        ctx.textBaseline = 'middle';
        const startY = Math.floor(minY / minorStep) * minorStep;

        for (let y = startY; y <= maxY; y += minorStep) {
            const py = (height / 2) - (y - centerY) / uPP;
            const isMajor = Math.abs(Math.round(y / majorStep) * majorStep - y) < minorStep * 0.1;

            ctx.beginPath();
            ctx.strokeStyle = isMajor ? '#8c8c8c' : '#cccccc';
            ctx.lineWidth = isMajor ? 1.5 : 1;
            ctx.moveTo(0, py);
            ctx.lineTo(width, py);
            ctx.stroke();

            if (isMajor && Math.abs(y) > 1e-9) {
                ctx.fillText(formatLabel(y), textX, py);
            }
        }

        ctx.beginPath();
        ctx.strokeStyle = '#000000';
        ctx.lineWidth = 1.5;
        if (xAxisY >= 0 && xAxisY <= height) { ctx.moveTo(0, xAxisY); ctx.lineTo(width, xAxisY); }
        if (yAxisX >= 0 && yAxisX <= width) { ctx.moveTo(yAxisX, 0); ctx.lineTo(yAxisX, height); }
        ctx.stroke();

        ctx.textAlign = 'right';
        ctx.textBaseline = 'top';
        ctx.fillText('0', textX, textY);
    },

    drawGraph: function (canvas, pointsData, color) {
        if (!canvas || !pointsData || pointsData.length < 4) return;
        const ctx = canvas.getContext('2d');
    
        ctx.beginPath();
        ctx.strokeStyle = color;
        ctx.lineWidth = 2;
        ctx.lineJoin = 'round';

        let started = false;

        for (let i = 0; i < pointsData.length; i += 2) {
            const x = pointsData[i];
            const y = pointsData[i + 1];

            if (y > -10000 && y < 10000 && x > -10000 && x < 10000) {
                if (!started) {
                    ctx.moveTo(x, y);
                    started = true;
                } else {
                    ctx.lineTo(x, y);
                }
            } else {
                started = false;
            }
        }
        ctx.stroke();
    },

    drawImplicit: function (canvas, segments, color) {
        if (!canvas || !segments) return;
        const ctx = canvas.getContext('2d');
        ctx.beginPath();
        ctx.strokeStyle = color;
        ctx.lineWidth = 2;
        for (let i = 0; i < segments.length; i+= 4) 
        {
            ctx.moveTo(segments[i], segments[i+1]);
            ctx.lineTo(segments[i+2], segments[i+3]);
        }
        ctx.stroke();
    },

    drawPoint: function (canvas, px, py, color) {
        if (!canvas) return;
        const ctx = canvas.getContext('2d');
        ctx.fillStyle = color;
        ctx.beginPath();
        ctx.arc(px, py, 5, 0, Math.PI * 2);
        ctx.fill();

        ctx.strokeStyle = 'white';
        ctx.lineWidth = 2;
        ctx.stroke();
    }
};