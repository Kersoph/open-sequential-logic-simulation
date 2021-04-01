namespace Osls
{
    public static class PersistenceCheckHelper
    {
        /// <summary>
        /// Binary sections are controlled by a serialized int which is checkeg against the expected value.
        /// </summary>
        public static bool CheckSectionNumber(System.IO.BinaryReader reader, int exceptedSection)
        {
            int sectionNumber = reader.ReadInt32();
            if (sectionNumber == exceptedSection) return true;
            Godot.GD.Print("CheckSectionNumber was invalid! " + sectionNumber
            + " != " + exceptedSection
            + " at pos " + reader.BaseStream.Position);
            Godot.GD.PrintStack();
            Godot.GD.PushError("CheckSectionNumber was invalid! " + sectionNumber
            + " != " + exceptedSection
            + " at pos " + reader.BaseStream.Position);
            return false;
        }

        /// <summary>
        /// Binary subsections are controlled by a serialized byte which is checked against the expected value.
        /// </summary>
        public static bool CheckSubsectionNumber(System.IO.BinaryReader reader, byte exceptedSubsection)
        {
            int sectionNumber = reader.ReadByte();
            if (sectionNumber == exceptedSubsection) return true;
            Godot.GD.Print("CheckSectionNumber was invalid! " + sectionNumber
            + " != " + exceptedSubsection
            + " at pos " + reader.BaseStream.Position);
            Godot.GD.PrintStack();
            Godot.GD.PushError("CheckSectionNumber was invalid! " + sectionNumber
            + " != " + exceptedSubsection
            + " at pos " + reader.BaseStream.Position);
            return false;
        }
    }
}