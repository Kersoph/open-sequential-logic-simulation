using System.Collections.Generic;
using System.IO;


namespace Osls.SfcEditor
{
    /// <summary>
    /// Holds the patch map for the patch entities
    /// </summary>
    public class SfcEntity
    {
        #region ==================== Fields / Properties ====================
        public const int XKeyShift = 16;
        public const int YKeyMask = 0b1111111111111111;
        public const int KeyOffset = 1 << 15;
        private readonly Dictionary<int, PatchEntity> _patchMap = new Dictionary<int, PatchEntity>();
        
        /// <summary>
        /// Gets the current patch collection of the sfc data
        /// </summary>
        public IReadOnlyCollection<PatchEntity> Patches { get { return _patchMap.Values; } }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Creates a new patch at the given position and adds it to the map.
        /// </summary>
        public void CreatePatchAt(short x, short y)
        {
            PatchEntity entity = new PatchEntity(x, y);
            AddPatch(entity);
        }
        
        /// <summary>
        /// Adds the patch to the data map
        /// </summary>
        public void AddPatch(PatchEntity patch)
        {
            int key = CalculateMapKey(patch.X, patch.Y);
            _patchMap.Add(key, patch);
        }
        
        /// <summary>
        /// Calculates the control map key from the given position.
        /// </summary>
        public static int CalculateMapKey(int x, int y)
        {
            int xKey = (KeyOffset + x) << XKeyShift;
            int yKey = (KeyOffset + y) & YKeyMask;
            return checked(xKey | yKey);
        }
        
        /// <summary>
        /// Tries to get the entity at the given position. Null if there is none.
        /// </summary>
        public PatchEntity Lookup(int x, int y)
        {
            int mapKey = CalculateMapKey(x, y);
            return Lookup(mapKey);
        }
        
        /// <summary>
        /// Tries to get the entity with the given key. Null if there is none.
        /// </summary>
        public PatchEntity Lookup(int key)
        {
            _patchMap.TryGetValue(key, out PatchEntity value);
            return value;
        }
        
        /// <summary>
        /// Writes the data from the stream. Read by "ReadFrom".
        /// </summary>
        public Godot.Error TryWriteTo(string filepath)
        {
            Godot.Error progress = Godot.Error.Ok;
            try
            {
                using (FileStream stream = File.Open(filepath, FileMode.OpenOrCreate))
                {
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        WriteTo(writer);
                    }
                }
            }
            catch (System.UnauthorizedAccessException)
            {
                progress = Godot.Error.FileNoPermission;
            }
            return progress;
        }
        
        /// <summary>
        /// Tries to load a new entity from the given filepath. Null if the path is invalid or we do not have access rights.
        /// </summary>
        public static SfcEntity TryLoadFromFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                return null;
            }
            try
            {
                return LoadFromFile(filepath);
            }
            catch (System.UnauthorizedAccessException)
            {
                return null;
            }
        }
        
        /// <summary>
        /// Loads a new entity from the given filepath. The path has to be valid.
        /// </summary>
        public static SfcEntity LoadFromFile(string filepath)
        {
            SfcEntity entity = new SfcEntity();
            using (FileStream stream = File.Open(filepath, FileMode.OpenOrCreate))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    entity.ReadFrom(reader);
                }
            }
            return entity;
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Loads the data from the stream. Written in "WriteTo".
        /// </summary>
        private void ReadFrom(BinaryReader reader)
        {
            PersistenceCheckHelper.CheckSectionNumber(reader, 0x11111111);
            _patchMap.Clear();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                PatchEntity entity = PatchEntity.CreateFrom(reader);
                AddPatch(entity);
            }
        }
        
        /// <summary>
        /// Writes the data from the stream. Read by "ReadFrom".
        /// </summary>
        private void WriteTo(BinaryWriter writer)
        {
            writer.Write(0x11111111);
            writer.Write(_patchMap.Count);
            foreach (PatchEntity entity in _patchMap.Values)
            {
                entity.WriteTo(writer);
            }
        }
        #endregion
    }
}
