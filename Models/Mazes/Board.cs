using Blazor.Extensions;
using Blazor.Extensions.Canvas;
using Blazor.Extensions.Canvas.Canvas2D;
using System.Threading.Tasks;
using System;
using System.Timers;

namespace RandomBlazorTests.Models.Mazes
{
    public class BoardManager
    {
        private readonly BECanvasComponent component;
        private Canvas2DContext canvas;
        private Timer Tick = new Timer();

        private Testing TestState = new Testing { Counter = 0 };

        public long Width => component.Width;
        public long Height => component.Height;

        public BoardManager(BECanvasComponent component)
        {
            this.component = component;
        }

        public async Task Init()
        {
            canvas = await component.CreateCanvas2DAsync();

            Tick.Interval = 1000;
            Tick.AutoReset = true;
            Tick.Elapsed += (s, e) => { TestState.Counter++; };
        }

        public void Start()
        {
            TestState.Counter = 0;
            Tick.Start();
        }

        public void Stop()
        {
            Tick.Stop();
        }

        public async Task Paint()
        {
            await canvas.BeginBatchAsync();
            await canvas.ClearRectAsync(0, 0, Width, Height);

            await canvas.SetFillStyleAsync("black");
            await canvas.FillRectAsync(0, 0, Width, Height);

            await canvas.SetFillStyleAsync("green");
            await canvas.FillRectAsync(10, 50, Width * 0.5, Height * 0.5);

            await canvas.SetFontAsync("10px serif");
            await canvas.StrokeTextAsync($"Hello Blazor!!!{DateTime.Now:g}", 10, 100);

            await canvas.StrokeTextAsync($"Running: {Tick.Enabled} Test: {TestState.Counter}", 10, 150);
            await canvas.EndBatchAsync();
        }

        private struct Testing
        {
            public int Counter { get; set; }
        }
    }
}