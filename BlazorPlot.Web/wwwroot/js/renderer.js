window.canvasRenderer = {
    clear: function (canvas) {
        if (!canvas) return;
        const ctx = canvas.getContext('2d');
        ctx.clearRect(0, 0, canvas.width, canvas.height);
    },

    drawAxes: function (canvas, centerX, centerY, uPP, width, height) {
        const ctx = canvas.getContext('2d');
        
        // net
        ctx.beginPath();
        ctx.strokeStyle = '#e0e0e0';
        ctx.lineWidth = 1;

        const step = 1;
        const startX = Math.floor((centerX - (width / 2) * uPP) / step) * step;
        const endX = Math.ceil((centerX + (width / 2) * uPP) / step) * step;

        for (let x = startX; x <= endX; x+=step) {
            const px = (width / 2) + (x - centerX) / uPP;
            ctx.moveTo(px, 0);
            ctx.lineTo(px, height);
        }

        const startY = Math.floor((centerY - (height / 2) * uPP) / step) * step;
        const endY = Math.ceil((centerY + (height / 2) * uPP) / step) * step;

        for (let y = startY; y <= endY; y+=step) {
            const py = (height / 2) - (y - centerY) / uPP;
            ctx.moveTo(0, py);
            ctx.lineTo(width, py);
        }

        ctx.stroke();

        // main OX OY
        ctx.beginPath();
        ctx.strokeStyle = '#000000';
        ctx.lineWidth = 2;

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