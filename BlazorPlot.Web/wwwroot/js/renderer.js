window.canvasRenderer = {
    clear: function (canvas) {
        if (!canvas) return;
        const ctx = canvas.getContext('2d');
        ctx.clearRect(0, 0, canvas.width, canvas.height);
    },

    drawGraph: function (canvas, pointsData, color) {
        if (!canvas || !pointsData || pointsData.length === 0) return;
        const ctx = canvas.getContext('2d');
        
        ctx.beginPath();
        ctx.strokeStyle = color;
        ctx.lineWidth = 2;
        ctx.lineJoin = 'round';

        let isFirstPoint = true;

        for (let i = 0; i < pointsData.length; i += 2) {
            const x = pointsData[i];
            const y = pointsData[i + 1];

            if (y > -10000 && y < 10000) {
                if (isFirstPoint) {
                    ctx.moveTo(x, y);
                    isFirstPoint = false;
                } else {
                    ctx.lineTo(x, y);
                }
            } else {
                isFirstPoint = true;
            }
        }
        ctx.stroke();
    }
};