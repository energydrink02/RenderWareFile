﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RenderWareFile
{
    public class MaterialList_0008 : RWSection
    {
        public MaterialListStruct_0001 materialListStruct;
        public Material_0007[] materialList;

        public MaterialList_0008 Read(BinaryReader binaryReader)
        {
            sectionIdentifier = Section.MaterialList;
            sectionSize = binaryReader.ReadInt32();
            renderWareVersion = binaryReader.ReadInt32();

            Section materialListStructSection = (Section)binaryReader.ReadInt32();
            if (materialListStructSection != Section.Struct) throw new Exception();
            materialListStruct = new MaterialListStruct_0001().Read(binaryReader);

            materialList = new Material_0007[materialListStruct.materialCount];

            for (int i = 0; i < materialListStruct.materialCount; i++)
            {
                Section materialSection = (Section)binaryReader.ReadInt32();
                if (materialSection != Section.Material) throw new Exception();
                materialList[i] = new Material_0007().Read(binaryReader);
            }

            return this;
        }
        
        public override void SetListBytes(int fileVersion, ref List<byte> listBytes)
        {
            sectionIdentifier = Section.MaterialList;

            listBytes.AddRange(materialListStruct.GetBytes(fileVersion));
            for (int i = 0; i < materialList.Count(); i++)
            {
                listBytes.AddRange(materialList[i].GetBytes(fileVersion));
            }
        }
    }

    public class MaterialListStruct_0001 : RWSection
    {
        public int materialCount;

        public MaterialListStruct_0001 Read(BinaryReader binaryReader)
        {
            sectionIdentifier = Section.Struct;
            sectionSize = binaryReader.ReadInt32();
            renderWareVersion = binaryReader.ReadInt32();

            materialCount = binaryReader.ReadInt32();
            for (int i = 0; i < materialCount; i++)
            {
                binaryReader.ReadInt32();
            }

            return this;
        }

        public override void SetListBytes(int fileVersion, ref List<byte> listBytes)
        {
            sectionIdentifier = Section.Struct;

            listBytes.AddRange(BitConverter.GetBytes(materialCount));
            for (int i = 0; i < materialCount; i++)
            {
                listBytes.AddRange(BitConverter.GetBytes(-1));
            }
        }
    }
}
