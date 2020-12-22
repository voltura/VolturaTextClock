using System.Drawing;
using System.Windows.Forms;

namespace QlocktwoClone
{
    public static class ComponentsExtensions
    {
        //Mouse drag and drop management
        #region Menu and Mouse

        private static bool m_MouseDown;
        private static Point m_LastLocation;

        /// <summary>
        /// To enable control to be moved around with mouse
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        public static void MoveSelfWithMouse<T>(this T control) where T : Control
        {
            control.MouseDown += (o, e) => { m_MouseDown = true; m_LastLocation = e.Location; };
            control.MouseMove += (o, e) =>
            {
                if (m_MouseDown)
                {
                    control.Location = new Point((control.Location.X - m_LastLocation.X) + e.X, (control.Location.Y - m_LastLocation.Y) + e.Y);
                    control.Update();
                }
            };
            control.MouseUp += (o, e) => { m_MouseDown = false; };
        }


        public static void MoveOtherWithMouse<T>(this T control, Control movedObject) where T : Control
        {
            control.MouseDown += (o, e) => { m_MouseDown = true; m_LastLocation = e.Location; };
            control.MouseMove += (o, e) =>
            {
                if (m_MouseDown)
                {
                    movedObject.Location = new Point((movedObject.Location.X - m_LastLocation.X) + e.X, (movedObject.Location.Y - m_LastLocation.Y) + e.Y);
                    movedObject.Update();
                }
            };
            control.MouseUp += (o, e) => { m_MouseDown = false; };
        }

        #endregion
    }
}
