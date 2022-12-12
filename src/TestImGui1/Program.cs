using System.Numerics;
using ImGuiNET;
using Veldrid;
using Veldrid.StartupUtilities;

var clearColor = new Vector3(0.45f, 0.55f, 0.6f);

var f = 0.0f;
var counter = 0;
var dragInt = 0;
var showImGuiDemoWindow = true;
var showAnotherWindow = false;
var showMemoryEditor = false;
byte[] memoryEditorData;
var s_tab_bar_flags = (uint)ImGuiTabBarFlags.Reorderable;
bool[] s_opened = { true, true, true, true }; // Persistent user state

var windowCreateInfo = new WindowCreateInfo(50, 50, 1280, 720, WindowState.Normal, "ImGui.NET Sample Program");

VeldridStartup.CreateWindowAndGraphicsDevice(
    windowCreateInfo,
    new GraphicsDeviceOptions(true, null, true, ResourceBindingModel.Improved, true, true),
    out var window,
    out var gd);
var cl = gd.ResourceFactory.CreateCommandList();
var controller = new ImGuiController(gd, gd.MainSwapchain.Framebuffer.OutputDescription, window.Width, window.Height);

window.Resized += () =>
{
    gd.MainSwapchain.Resize((uint)window.Width, (uint)window.Height);
    controller.WindowResized(window.Width, window.Height);
};

while (window.Exists)
{
    var snapshot = window.PumpEvents();
    if (!window.Exists) { break; }
    controller.Update(1f / 60f, snapshot);
    
    SubmitUi();

    cl.Begin();
    cl.SetFramebuffer(gd.MainSwapchain.Framebuffer);
    cl.ClearColorTarget(0, new RgbaFloat(clearColor.X, clearColor.Y, clearColor.Z, 1f));
    controller.Render(gd, cl);
    cl.End();
    gd.SubmitCommands(cl);
    gd.SwapBuffers(gd.MainSwapchain);
}


void SubmitUi()
{
    {
        ImGui.Begin("Test");
        ImGui.Text("Hello, world!"); // Display some text (you can use a format string too)
        ImGui.SliderFloat("float", ref f, 0, 1,
            f.ToString("0.000")); // Edit 1 float using a slider from 0.0f to 1.0f    
        //ImGui.ColorEdit3("clear color", ref _clearColor);                   // Edit 3 floats representing a color

        ImGui.Text($"Mouse position: {ImGui.GetMousePos()}");

        ImGui.Checkbox("ImGui Demo Window", ref showImGuiDemoWindow); // Edit bools storing our windows open/close state
        ImGui.Checkbox("Another Window", ref showAnotherWindow);
        ImGui.Checkbox("Memory Editor", ref showMemoryEditor);
        if (ImGui.Button(
                "Button")) // Buttons return true when clicked (NB: most widgets return true when edited/activated)
            counter++;
        ImGui.SameLine(0, -1);
        ImGui.Text($"counter = {counter}");

        ImGui.DragInt("Draggable Int", ref dragInt);

        var framerate = ImGui.GetIO().Framerate;
        ImGui.Text($"Application average {1000.0f / framerate:0.##} ms/frame ({framerate:0.#} FPS)");
    }

    {
        ImGui.ShowDemoWindow();
    }
}