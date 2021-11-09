using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace SpaceInv
{
    static class EntityManager
    {
        static List<Entity> entities = new List<Entity>();
        static Queue<Missile> missiles = new Queue<Missile>();

        static bool isUpdating;
        static List<Entity> addedEntities = new List<Entity>();
        public static void Add(Entity entity)
        {
            if (!isUpdating)
                AddEntity(entity);
            else
                addedEntities.Add(entity);
        }
        private static void AddEntity(Entity entity)
        {
            entities.Add(entity);
            if (entity is Missile)
                Missiles.Enqueue(entity as Missile);
        }
        public static int Count { get { return entities.Count; } }

        internal static Queue<Missile> Missiles { get => missiles; set => missiles = value; }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
                entity.Draw(spriteBatch);
        }

        public static void Update()
        {
            isUpdating = true;

            // update all objects in queue
            foreach (var entity in entities)
            {
                entity.Update();
            }           

            isUpdating = false;

            foreach (var entity in addedEntities)
            {
                AddEntity(entity);
            } 

            addedEntities.Clear();

            List<Entity> entitiesT = new List<Entity>(entities.Where(entity => !entity.IsExpired).ToList());
            entities.Clear();
            entities = entitiesT;

        }

        public static void IsShot()
        {
            // Is alien or player shot by bullet
        }

        public static void Destroy(Entity entity)
        {
            entities.Remove(entity);
        }
        public static void DestroyMissile(Entity entity)
        {
            Missiles.Dequeue();
        }
    }
}
