using AvalonDock;

namespace Demacia.Services
{
    public static class DocumentService
    {
        public static DockingManager DockingManager { get; set; }

        public static void ShowDocument(DocumentContent doc, bool floating)
        {
            doc.Show(DockingManager, floating);
            doc.Activate();
        }

        public static void ShowDockable(DockableContent doc)
        {
            doc.Show(DockingManager);
            doc.Activate();
        }
    }
}
