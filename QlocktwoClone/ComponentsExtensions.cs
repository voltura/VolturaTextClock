using System.Windows.Forms;

namespace QlocktwoClone
{
    public static class ComponentsExtensions
    {
        private static bool m_MouseDown;
        private static System.Drawing.Point m_LastLocation;

        public static void MoveOtherWithMouse<T>(this T control, Control movedObject) where T : Control
        {
            control.MouseDown += (o, e) => { m_MouseDown = true; m_LastLocation = e.Location; };
            control.MouseMove += (o, e) =>
            {
                if (m_MouseDown)
                {
                    //movedObject.Location = new System.Drawing.Point(movedObject.Location.X - m_LastLocation.X + e.X, movedObject.Location.Y - m_LastLocation.Y + e.Y);
                    movedObject.Left = movedObject.Location.X - m_LastLocation.X + e.X;
                    movedObject.Top = movedObject.Location.Y - m_LastLocation.Y + e.Y;
                    movedObject.Update();
                }
            };
            control.MouseUp += (o, e) => { m_MouseDown = false; };
        }
    }
}
