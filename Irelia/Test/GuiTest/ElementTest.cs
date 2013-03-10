using System.Xml;
using Irelia.Gui;
using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GuiTest
{
    [TestClass()]
    public class ElementTest
    {
        private Device Device { get; set; }
        private AssetManager AssetManager { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
            AssetManager = new AssetManager(Device, RenderSettings.MediaPath);
        }

        private class ElementMock : Element
        {
            public ElementMock(IElement parent, AssetManager assetManager)
                : base(ElementType.Custom, parent, assetManager)
            {
            }

            protected override bool OnRender(SpriteRenderer spriteRenderer)
            {
                return true;
            }

            protected override object OnReadXml(XmlReader reader) { return null; }
            protected override void OnWriteXml(XmlWriter writer) { }
        }

        [TestMethod()]
        public void Element_DefaultValues_Test()
        {
            var parent = new TestHelpers.RootElement();
            var element = new ElementMock(parent, AssetManager) { Name = "ElementMock" };
            Assert.AreEqual("ElementMock", element.Name);
            Assert.AreEqual(new Rectangle(0.0f, 0.0f, 1.0f, 1.0f), element.DestRect);
            var expectedAbsRect = new Rectangle(parent.AbsRect.Left + parent.AbsRect.Width * element.DestRect.Left,
                                                parent.AbsRect.Top + parent.AbsRect.Height * element.DestRect.Top,
                                                parent.AbsRect.Width * element.DestRect.Width,
                                                parent.AbsRect.Height * element.DestRect.Height);
            Assert.AreEqual(expectedAbsRect, element.AbsRect);
            Assert.IsTrue(element.IsLogical);
        }

        [TestMethod()]
        public void Element_ParentChangesPositionAndSize_Test()
        {
            var parent = new TestHelpers.RootElement();
            var element = new ElementMock(parent, AssetManager) { Name = "ElementMock" };

            parent.AbsRect = new Rectangle(10.0f, 20.0f, 110.0f, 120.0f);
            
            var expectedAbsRect = new Rectangle(parent.AbsRect.Left + parent.AbsRect.Width * element.DestRect.Left,
                                                parent.AbsRect.Top + parent.AbsRect.Height * element.DestRect.Top,
                                                parent.AbsRect.Width * element.DestRect.Width,
                                                parent.AbsRect.Height * element.DestRect.Height);
            Assert.AreEqual(expectedAbsRect, element.AbsRect);
        }

        [TestMethod()]
        public void Element_MouseButtonDown_Test()
        {
            var element = new ElementMock(new TestHelpers.RootElement(), AssetManager)
            {
                DestRect = new Rectangle(0.5f, 0.5f, 0.5f, 0.5f)
            };

            bool leftDown = false, rightDown = false, middleDown = false;
            element.MouseDown += ((o, e) => 
                {
                    switch (e.ChangedButton)
                    {
                        case MouseButton.Left:
                            leftDown = true;
                            break;
                        case MouseButton.Right:
                            rightDown = true;
                            break;
                        case MouseButton.Middle:
                            middleDown = true;
                            break;
                    }
                    e.Handled = true;
                });

            element.InjectMouseDownEvent(element.AbsRect.Left - 1, element.AbsRect.Top - 1, MouseButton.Left);
            Assert.IsFalse(leftDown);
            element.InjectMouseDownEvent(element.AbsRect.Left, element.AbsRect.Top, MouseButton.Left);
            Assert.IsTrue(leftDown);
            
            element.InjectMouseDownEvent(element.AbsRect.Left - 1, element.AbsRect.Top - 1, MouseButton.Right);
            Assert.IsFalse(rightDown);
            element.InjectMouseDownEvent(element.AbsRect.Left, element.AbsRect.Top, MouseButton.Right);
            Assert.IsTrue(rightDown);
            
            element.InjectMouseDownEvent(element.AbsRect.Left - 1, element.AbsRect.Top - 1, MouseButton.Middle);
            Assert.IsFalse(middleDown);
            element.InjectMouseDownEvent(element.AbsRect.Left, element.AbsRect.Top, MouseButton.Middle);
            Assert.IsTrue(middleDown);
        }

        [TestMethod()]
        public void Element_MouseButtonUp_Test()
        {
            var element = new ElementMock(new TestHelpers.RootElement(), AssetManager)
            {
                DestRect = new Rectangle(0.5f, 0.5f, 0.5f, 0.5f)
            };

            bool leftUp = false, rightUp = false, middleUp = false;
            element.MouseUp += ((o, e) =>
            {
                switch (e.ChangedButton)
                {
                    case MouseButton.Left:
                        leftUp = true;
                        break;
                    case MouseButton.Right:
                        rightUp = true;
                        break;
                    case MouseButton.Middle:
                        middleUp = true;
                        break;
                }
                e.Handled = true;
            });

            element.InjectMouseUpEvent(element.AbsRect.Left - 1, element.AbsRect.Top - 1, MouseButton.Left);
            Assert.IsFalse(leftUp);
            element.InjectMouseUpEvent(element.AbsRect.Left, element.AbsRect.Top, MouseButton.Left);
            Assert.IsTrue(leftUp);

            element.InjectMouseUpEvent(element.AbsRect.Left - 1, element.AbsRect.Top - 1, MouseButton.Right);
            Assert.IsFalse(rightUp);
            element.InjectMouseUpEvent(element.AbsRect.Left, element.AbsRect.Top, MouseButton.Right);
            Assert.IsTrue(rightUp);

            element.InjectMouseUpEvent(element.AbsRect.Left - 1, element.AbsRect.Top - 1, MouseButton.Middle);
            Assert.IsFalse(middleUp);
            element.InjectMouseUpEvent(element.AbsRect.Left, element.AbsRect.Top, MouseButton.Middle);
            Assert.IsTrue(middleUp);
        }

        [TestMethod()]
        public void Element_OnlyLastestChildFiresEvent_Test()
        {
            // Direct routing strategy
            bool parentFired = false;
            var element = new ElementMock(new TestHelpers.RootElement(), AssetManager);
            element.MouseDown += ((o, e) => parentFired = true);

            bool childFired = false;
            var child = new ElementMock(element, AssetManager);
            child.MouseDown += (o, e) =>
                {
                    childFired = true;
                    e.Handled = true;
                };

            element.InjectMouseDownEvent(0.0f, 0.0f, MouseButton.Left);
            Assert.IsFalse(parentFired);
            Assert.IsTrue(childFired);
        }

        [TestMethod()]
        public void Element_MouseMove_Test()
        {
            var parent = new TestHelpers.RootElement();
            var element = new ElementMock(parent, AssetManager)
            {
                DestRect = new Rectangle(0.5f, 0.5f, 0.5f, 0.5f)
            };
            var elementPos = new Vector2(); // element-relative position
            var parentPos = new Vector2();  // parent(root)-relative position
            element.MouseMove += (o, e) => 
                {
                    elementPos = e.GetPosition(element);
                    parentPos = e.GetPosition(parent);
                };

            float moveX = 0.6f * parent.AbsRect.Width,
                  moveY = 0.7f * parent.AbsRect.Height;
            element.InjectMouseMoveEvent(moveX, moveY);
            Assert.AreEqual(moveX, parentPos.x);
            Assert.AreEqual(moveY, parentPos.y);
            Assert.AreEqual(moveX - element.AbsRect.Left, elementPos.x);
            Assert.AreEqual(moveY - element.AbsRect.Top, elementPos.y);
        }

        [TestMethod()]
        public void Element_MouseEnterLeave_Test()
        {
            var parent = new TestHelpers.RootElement();
            var element = new ElementMock(parent, AssetManager)
            {
                DestRect = new Rectangle(0.5f, 0.5f, 0.5f, 0.5f)
            };
            bool entered = false, left = false;
            element.MouseEnter += ((o, e) => entered = true);
            element.MouseLeave += ((o, e) => left = true);

            element.InjectMouseMoveEvent(0.0f, 0.0f);
            Assert.IsFalse(entered);

            element.InjectMouseMoveEvent(element.AbsRect.Left + 1, element.AbsRect.Top + 1);
            Assert.IsTrue(entered);
            Assert.IsFalse(left);

            element.InjectMouseMoveEvent(0.0f, 0.0f);
            Assert.IsTrue(left);
        }

        [TestMethod()]
        public void Element_RemoveFromParent_Test()
        {
            var parent = new TestHelpers.RootElement();
            var element = new ElementMock(parent, AssetManager);
            var element2 = new ElementMock(element, AssetManager);

            // Element whose parent is root cannot be removed 
            Assert.IsFalse(element.RemoveFromParent());

            Assert.IsTrue(element2.RemoveFromParent());
        }

        [TestMethod()]
        public void Element_GetVisualTree_Test()
        {
            var parent = new TestHelpers.RootElement();
            var element = new ElementMock(parent, AssetManager);
            var element2 = new ElementMock(element, AssetManager);
            var element3 = new ElementMock(element2, AssetManager);

            var expected = new Element[] { element, element2, element3 };
            Assert.IsTrue(expected.SequenceEqual(element.GetVisualTree()));
        }

        [TestMethod()]
        public void Element_IsVisible_Test()
        {
            var parent = new TestHelpers.RootElement();
            var element = new ElementMock(parent, AssetManager);
            var element2 = new ElementMock(element, AssetManager);
            Assert.IsTrue(element.IsVisible);
            
            element.IsVisible = false;
            Assert.IsFalse(element2.IsVisible);
            TestHelpers.RenderSprite(Device, s => Assert.IsFalse((element as ISprite).Render(s)));

            var element3 = new ElementMock(element2, AssetManager);
            Assert.IsFalse(element3.IsVisible);
        }

        [TestMethod()]
        public void Element_GetElement_Test()
        {
            var element = new ElementMock(new TestHelpers.RootElement(), AssetManager);

            string elementName = "element2";
            var expected = new ElementMock(element, AssetManager) { Name = elementName };
            Assert.AreEqual(expected, element.GetElement(elementName));
            Assert.IsNull(element.GetElement("invalidName"));
        }

        [TestMethod()]
        public void Element_TopMost_Test()
        {
            var element = new ElementMock(new TestHelpers.RootElement(), AssetManager);
        }
    }
}
