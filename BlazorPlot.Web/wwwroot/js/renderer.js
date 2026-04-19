window.canvasRenderer = {
	drawGraph: function (canvas, pointsData) {
		if (!canvas) return;
		const ctx = canvas.getContext('2d');

		ctx.clearRect(0, 0, canvas.width, canvas.height);
		ctx.beginPath();
		ctx.strokeStyle = '#0078D7';
		ctx.lineWidth = 2;

		for (let i = 0; i < pointsData.length; i += 2) {
			const x = pointsData[i];
			const y = pointsData[i + 1];

			if (i === 0) {
				ctx.moveTo(x, y);
			} else {
				ctx.lineTo(x, y);
			}
		}
		ctx.stroke();
	}
};