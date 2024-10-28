using System.Drawing;

public class DirectionalLight : Light
{
    public DirectionalLight()
    {
        Color color = Color.LightYellow;

        CircleRenderer circleRenderer = AddComponent<CircleRenderer>();
        circleRenderer.Material.Color = color;
        Color = color;
    }
}