using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.objects;

namespace NettyBaseReloaded.Utils.form
{
    class MapDrawing
    {
        private static Spacemap Map { get; set; }

        private static SizeF Size { get; set; }

        private static Graphics Gfx { get; set; }

        private static bool ToggledCollectables { get; set; }
        private static bool MoreCalculus { get; set; }

        private static PointF MousePosition { get; set; }

        public static void Initiate(Spacemap map, object sender, PaintEventArgs e, bool toggledObjects, bool toggledPOI, bool toggledEntities, bool toggledCollectables, bool moreCalculus, Point mousePos)
        {
            Size = e.ClipRectangle.Size;
            Map = map;
            Gfx = e.Graphics;
            ToggledCollectables = toggledCollectables;
            MoreCalculus = moreCalculus;
            MousePosition = mousePos;

            if (toggledObjects)
                DrawObjects();
            if (toggledPOI)
                DrawPOI();
            if (toggledEntities)
                DrawEntities();
        }

        private static void DrawObjects()
        {
            var pen = new Pen(Color.White, 1);
            foreach (var mapObject in Map.Objects.Values)
            {
                if (mapObject is Jumpgate)
                {
                    Gfx.DrawEllipse(pen, new RectangleF(GetDrawingPointF(mapObject.Position), new SizeF(6.5f,6.5f)));
                }
                else if (mapObject is Station)
                {
                    var station = (Station) mapObject;
                    var brush = new SolidBrush(Color.Gray);
                    if (station.Faction == Faction.MMO) brush = new SolidBrush(Color.DarkRed);
                    else if (station.Faction == Faction.EIC) brush = new SolidBrush(Color.LightSkyBlue);
                    else if (station.Faction == Faction.VRU) brush = new SolidBrush(Color.ForestGreen);

                    Gfx.FillRectangle(brush, new RectangleF(GetDrawingPointF(mapObject.Position), new Size(10,10)));
                }
                else if (mapObject is Collectable && ToggledCollectables)
                {
                    var brush = new SolidBrush(Color.Yellow);
                    Gfx.FillRectangle(brush, new RectangleF(GetDrawingPointF(mapObject.Position), new SizeF(2.5f,2.5f)));
                }
            }
        }

        private static void DrawPOI()
        {
            var brush = new TextureBrush(Properties.Resources.gray_diagonal_stripes_seamless_background_pattern);
            foreach (var poi in Map.POIs.Values)
            {
                var orderedCords = poi.ShapeCords.OrderBy(x => x.X).ThenBy(y => y.Y).ToList();
                var topLeft = GetDrawingPointF(orderedCords[0]);
                var botRight = GetDrawingPointF(orderedCords[3]);
                Gfx.FillRectangle(brush, new RectangleF(topLeft, new SizeF(botRight.X - topLeft.X, botRight.Y - topLeft.Y)));
            }
        }

        private static void DrawEntities()
        {
            foreach (var entity in Map.Entities.Values)
            {
                Brush entityBrush = null;
                switch (entity.FactionId)
                {
                    case Faction.MMO:
                        entityBrush = new SolidBrush(Color.DarkRed);
                        break;
                    case Faction.EIC:
                        entityBrush = new SolidBrush(Color.LightSkyBlue);
                        break;
                    case Faction.VRU:
                        entityBrush = new SolidBrush(Color.Green);
                        break;
                    default:
                        entityBrush = new SolidBrush(Color.Chocolate);
                        break;
                }

                if (entity.Controller.Attack.Attacking)
                {
                    entityBrush = new SolidBrush(Color.Red);
                }
                var trigBase = entity.Position;
                if (MoreCalculus)
                {
                    trigBase = MovementController.ActualPosition(entity);
                }
                
                var pointArray = new PointF[]
                {
                    GetDrawingPointF(new Vector(trigBase.X - 150, trigBase.Y - 150)), GetDrawingPointF(new Vector(trigBase.X, trigBase.Y + 150)),
                    GetDrawingPointF(new Vector(trigBase.X + 150, trigBase.Y - 150))
                };

                float x = pointArray.Select(_ => _.X).Sum() / pointArray.Length;
                float y = pointArray.Select(_ => _.Y).Sum() / pointArray.Length;

                if (MoreCalculus)
                {
                    using (Matrix m = new Matrix())
                    {
                        m.RotateAt((float) trigBase.GetAngle(entity.Direction), new PointF(x, y));
                        Gfx.Transform = m;
                        Gfx.FillPolygon(entityBrush, pointArray);
                        Gfx.ResetTransform();
                    }
                    Gfx.SmoothingMode = SmoothingMode.AntiAlias;
                }
                else Gfx.FillPolygon(entityBrush, pointArray);
                if (entity.Controller.Attack.Attacking)
                    DrawShootingLine(entity, entity.Selected);

                if (DistanceTo(GetDrawingPointF(entity.Position), MousePosition) <= 5)
                {
                    Gfx.DrawString($"{entity.Id}:{entity.Name}\n{entity.Position.X},{entity.Position.Y}\n{entity.CurrentHealth}/{entity.MaxHealth}\n{entity.CurrentNanoHull}/{entity.MaxNanoHull}\n{entity.CurrentShield}/{entity.MaxShield}", new Font(FontFamily.GenericMonospace, 6f), new SolidBrush(Color.White), GetDrawingPointF(new Vector(trigBase.X + 200, trigBase.Y + 300)));
                    DrawDestination(entity);
                }
            }
        }

        private static PointF GetDrawingPointF(Vector pos)
        {
            var xm = Size.Width / Map.Limits[1].X;
            var ym = Size.Height / Map.Limits[1].Y;
            return new PointF(pos.X * xm, pos.Y * ym);
        }

        private static void DrawShootingLine(Character from, IAttackable to)
        {
            if (to == null || from == null) return;

            Gfx.DrawLine(new Pen(Color.White, 1f), GetDrawingPointF(from.Position), GetDrawingPointF(to.Position));
        }

        private static double DistanceTo(PointF point1, PointF point2)
        {
            return Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));
        }

        private static void DrawDestination(Character character)
        {
            var dest = character.Destination;
            var pos = character.Position;
            var half = new Vector((dest.X + pos.X) / 2, (dest.Y + pos.Y) / 2);
            var pen = new Pen(Color.FromArgb(75, 255, 255, 255), 1);
            pen.EndCap = LineCap.ArrowAnchor;
            var pen2 = new Pen(Color.FromArgb(75, 255, 255, 255), 1);
            Gfx.DrawLine(pen, GetDrawingPointF(pos), GetDrawingPointF(half));
            Gfx.DrawLine(pen2, GetDrawingPointF(half), GetDrawingPointF(dest));
        }
    }
}
