window.canvasRenderer = {
    clear: function (canvas) {
        if (!canvas) return;
        const ctx = canvas.getContext('2d');
        ctx.clearRect(0, 0, canvas.width, canvas.height);
    },

    drawAxes: function (canvas, centerX, centerY, uPP, width, height) {
        const ctx = canvas.getContext('2d');
        
        const visibleWidth = width * uPP;

        const magnitude = Math.pow(10, Math.floor(Math.log10(visibleWidth / 5)));
        let step = magnitude;

        if (visibleWidth / step > 15) step *= 5;
        else if (visibleWidth / step > 8) step *= 2;

        if (step <= 0) step = 1;

        ctx.font = '12px monospace';
        ctx.fillStyle = '#666';
        
        // net
        ctx.beginPath();
        ctx.strokeStyle = '#e0e0e0';
        ctx.lineWidth = 1;

        const xAxisY = (height / 2) + centerY / uPP;
        const yAxisX = (width / 2) - centerX / uPP;
       
        const startX = Math.floor((centerX - (width / 2) * uPP) / step) * step;
        const endX = Math.ceil((centerX + (width / 2) * uPP) / step) * step;
       
        ctx.textAlign = 'center';
        ctx.textBaseline = 'top';

        let textY = xAxisY + 5;
        if (textY < 5) textY = 5;
        if (textY > height - 20) textY = height - 20;

        for (let x = startX; x <= endX; x+=step) {
            const px = (width / 2) + (x - centerX) / uPP;
            
            ctx.moveTo(px, 0);
            ctx.lineTo(px, height);

            if (Math.abs(x) > 1e-9) {
                let label = parseFloat(x.toPrecision(12)).toString();
                ctx.fillText(label, px, textY);
            }
        }

        const startY = Math.floor((centerY - (height / 2) * uPP) / step) * step;
        const endY = Math.ceil((centerY + (height / 2) * uPP) / step) * step;

        ctx.textAlign = 'right';
        ctx.textBaseline = 'middle';

        let textX = yAxisX -5;
        if (textX < 30) textX = 30;
        if (textX > width - 5) textX = width - 5;

        for (let y = startY; y <= endY; y+=step) {
            const py = (height / 2) - (y - centerY) / uPP;
            
            ctx.moveTo(0, py);
            ctx.lineTo(width, py);

            if (Math.abs(y) > 1e-9) {
                let label = parseFloat(y.toPrecision(12)).toString();
                ctx.fillText(label, textX, py);
            }
        }

        ctx.stroke();

        // main OX OY
        ctx.beginPath();
        ctx.strokeStyle = '#000000';
        ctx.lineWidth = 1.5;

        if (xAxisY >=0 && xAxisY <= height) {
            ctx.moveTo(0, xAxisY);
            ctx.lineTo(width, xAxisY);
        }

        if (yAxisX >=0 && yAxisX <= width) {
            ctx.moveTo(yAxisX, 0);
            ctx.lineTo(yAxisX, height);
        }
        ctx.stroke();

        ctx.textAlign = 'right';
        ctx.textBaseline = 'top';
        ctx.fillText('0', textX, textY);
        
        const axisY = (height / 2) + centerY / uPP;
        ctx.moveTo(0, axisY);
        ctx.lineTo(width, axisY);

        const axisX = (width / 2) - centerX / uPP;
        ctx.moveTo(axisX, 0);
        ctx.lineTo(axisX, height);

        ctx.stroke();
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
    }
};