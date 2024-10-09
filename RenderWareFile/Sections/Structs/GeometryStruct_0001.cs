﻿using System;
using System.Collections.Generic;
using System.IO;

namespace RenderWareFile.Sections
{
    public struct MorphTarget
    {
        public Vertex3 sphereCenter;
        public float radius;
        public int hasVertices;
        public int hasNormals;

        public Vertex3[] vertices;
        public Vertex3[] normals;
    }

    public class GeometryStruct_0001 : RWSection
    {
        public GeometryFlags geometryFlags;
        public int numTriangles;
        public int numVertices;
        public int numMorphTargets;

        public float ambient;
        public float specular;
        public float diffuse;

        public float sphereCenterX;
        public float sphereCenterY;
        public float sphereCenterZ;
        public float sphereRadius;
        public int unknown1;
        public int unknown2;

        public Color[] vertexColors;
        public Vertex2[] textCoords;
        public Vertex2[] textCoords2;
        public Triangle[] triangles;
        public MorphTarget[] morphTargets;

        public byte[] sectionData;

        public GeometryStruct_0001 Read(BinaryReader binaryReader)
        {
            sectionIdentifier = Section.Struct;
            sectionSize = binaryReader.ReadInt32();
            renderWareVersion = binaryReader.ReadInt32();

            if (ReadFileMethods.treatStuffAsByteArray)
            {
                sectionData = binaryReader.ReadBytes(sectionSize);
                return this;
            }

            long startSectionPosition = binaryReader.BaseStream.Position;

            geometryFlags = (GeometryFlags)binaryReader.ReadInt32();
            numTriangles = binaryReader.ReadInt32();
            numVertices = binaryReader.ReadInt32();
            numMorphTargets = binaryReader.ReadInt32();

            int numTexCoords = (geometryFlags & GeometryFlags.rpGEOMETRYTEXTURED) != 0 ? 1 : (geometryFlags & GeometryFlags.rpGEOMETRYTEXTURED2) != 0 ? 2 : ((int)geometryFlags >> 16) & 0xFF;

            if (Shared.UnpackLibraryVersion(renderWareVersion) < 0x34000)
            {
                ambient = binaryReader.ReadSingle();
                specular = binaryReader.ReadSingle();
                diffuse = binaryReader.ReadSingle();
            }

            if ((geometryFlags & GeometryFlags.rpGEOMETRYNATIVE) != 0)
            {
                sphereCenterX = binaryReader.ReadSingle();
                sphereCenterY = binaryReader.ReadSingle();
                sphereCenterZ = binaryReader.ReadSingle();
                sphereRadius = binaryReader.ReadSingle();
                unknown1 = binaryReader.ReadInt32();
                unknown2 = binaryReader.ReadInt32();

                return this;
            }

            if ((geometryFlags & GeometryFlags.rpGEOMETRYPRELIT) != 0)
            {
                vertexColors = new Color[numVertices];
                for (int i = 0; i < numVertices; i++)
                {
                    vertexColors[i] = new Color()
                    {
                        R = binaryReader.ReadByte(),
                        G = binaryReader.ReadByte(),
                        B = binaryReader.ReadByte(),
                        A = binaryReader.ReadByte()
                    };
                }
            }

            if (numTexCoords >= 1)
            {
                textCoords = new Vertex2[numVertices];
                for (int i = 0; i < numVertices; i++)
                    textCoords[i] = new Vertex2()
                    {
                        X = binaryReader.ReadSingle(),
                        Y = binaryReader.ReadSingle()
                    };
            }

            if (numTexCoords >= 2)
            {
                textCoords2 = new Vertex2[numVertices];
                for (int i = 0; i < numVertices; i++)
                    textCoords2[i] = new Vertex2()
                    {
                        X = binaryReader.ReadSingle(),
                        Y = binaryReader.ReadSingle()
                    };

                binaryReader.BaseStream.Position += (numVertices * 8) * (numTexCoords - 2);
            }

            triangles = new Triangle[numTriangles];
            for (int i = 0; i < numTriangles; i++)
            {
                triangles[i] = new Triangle()
                {
                    vertex2 = binaryReader.ReadUInt16(),
                    vertex1 = binaryReader.ReadUInt16(),
                    materialIndex = binaryReader.ReadUInt16(),
                    vertex3 = binaryReader.ReadUInt16()
                };
            }

            morphTargets = new MorphTarget[numMorphTargets];
            for (int i = 0; i < numMorphTargets; i++)
            {
                MorphTarget m = new MorphTarget();

                m.sphereCenter.X = binaryReader.ReadSingle();
                m.sphereCenter.Y = binaryReader.ReadSingle();
                m.sphereCenter.Z = binaryReader.ReadSingle();
                m.radius = binaryReader.ReadSingle();
                m.hasVertices = binaryReader.ReadInt32();
                m.hasNormals = binaryReader.ReadInt32();

                if (m.hasVertices != 0)
                {
                    m.vertices = new Vertex3[numVertices];
                    for (int j = 0; j < numVertices; j++)
                    {
                        m.vertices[j] = new Vertex3()
                        {
                            X = binaryReader.ReadSingle(),
                            Y = binaryReader.ReadSingle(),
                            Z = binaryReader.ReadSingle()
                        };
                    }
                }

                if (m.vertices == null)
                    throw new Exception();

                if (m.hasNormals != 0)
                {
                    m.normals = new Vertex3[numVertices];
                    for (int j = 0; j < numVertices; j++)
                    {
                        m.normals[j] = new Vertex3()
                        {
                            X = binaryReader.ReadSingle(),
                            Y = binaryReader.ReadSingle(),
                            Z = binaryReader.ReadSingle()
                        };
                    }
                }

                morphTargets[i] = m;
            }

            return this;
        }

        public override void SetListBytes(int fileVersion, ref List<byte> listBytes)
        {
            sectionIdentifier = Section.Struct;

            if (ReadFileMethods.treatStuffAsByteArray)
            {
                listBytes.AddRange(sectionData);
                return;
            }

            listBytes.AddRange(BitConverter.GetBytes((int)geometryFlags));
            listBytes.AddRange(BitConverter.GetBytes(numTriangles));
            listBytes.AddRange(BitConverter.GetBytes(numVertices));
            listBytes.AddRange(BitConverter.GetBytes(numMorphTargets));

            if (Shared.UnpackLibraryVersion(fileVersion) < 0x34000)
            {
                listBytes.AddRange(BitConverter.GetBytes(ambient));
                listBytes.AddRange(BitConverter.GetBytes(specular));
                listBytes.AddRange(BitConverter.GetBytes(diffuse));
            }

            if ((geometryFlags & GeometryFlags.rpGEOMETRYNATIVE) != 0)
            {
                listBytes.AddRange(BitConverter.GetBytes(sphereCenterX));
                listBytes.AddRange(BitConverter.GetBytes(sphereCenterY));
                listBytes.AddRange(BitConverter.GetBytes(sphereCenterZ));
                listBytes.AddRange(BitConverter.GetBytes(sphereRadius));
                listBytes.AddRange(BitConverter.GetBytes(unknown1));
                listBytes.AddRange(BitConverter.GetBytes(unknown2));
            }
            else
            {
                if ((geometryFlags & GeometryFlags.rpGEOMETRYPRELIT) != 0)
                {
                    for (int i = 0; i < numVertices; i++)
                    {
                        listBytes.Add(vertexColors[i].R);
                        listBytes.Add(vertexColors[i].G);
                        listBytes.Add(vertexColors[i].B);
                        listBytes.Add(vertexColors[i].A);
                    }
                }

                if ((geometryFlags & (GeometryFlags.rpGEOMETRYTEXTURED | GeometryFlags.rpGEOMETRYTEXTURED2)) != 0)
                {
                    for (int i = 0; i < numVertices; i++)
                    {
                        listBytes.AddRange(BitConverter.GetBytes(textCoords[i].X));
                        listBytes.AddRange(BitConverter.GetBytes(textCoords[i].Y));
                    }
                }

                if ((geometryFlags & GeometryFlags.rpGEOMETRYTEXTURED2) != 0)
                {
                    for (int i = 0; i < numVertices; i++)
                    {
                        listBytes.AddRange(BitConverter.GetBytes(textCoords2[i].X));
                        listBytes.AddRange(BitConverter.GetBytes(textCoords2[i].Y));
                    }
                }

                for (int i = 0; i < numTriangles; i++)
                {
                    listBytes.AddRange(BitConverter.GetBytes(triangles[i].vertex2));
                    listBytes.AddRange(BitConverter.GetBytes(triangles[i].vertex1));
                    listBytes.AddRange(BitConverter.GetBytes(triangles[i].materialIndex));
                    listBytes.AddRange(BitConverter.GetBytes(triangles[i].vertex3));
                }

                for (int i = 0; i < numMorphTargets; i++)
                {
                    listBytes.AddRange(BitConverter.GetBytes(morphTargets[i].sphereCenter.X));
                    listBytes.AddRange(BitConverter.GetBytes(morphTargets[i].sphereCenter.Y));
                    listBytes.AddRange(BitConverter.GetBytes(morphTargets[i].sphereCenter.Z));
                    listBytes.AddRange(BitConverter.GetBytes(morphTargets[i].radius));
                    listBytes.AddRange(BitConverter.GetBytes(morphTargets[i].hasVertices));
                    listBytes.AddRange(BitConverter.GetBytes(morphTargets[i].hasNormals));

                    if (morphTargets[i].hasVertices != 0)
                    {
                        for (int j = 0; j < numVertices; j++)
                        {
                            listBytes.AddRange(BitConverter.GetBytes(morphTargets[i].vertices[j].X));
                            listBytes.AddRange(BitConverter.GetBytes(morphTargets[i].vertices[j].Y));
                            listBytes.AddRange(BitConverter.GetBytes(morphTargets[i].vertices[j].Z));
                        }
                    }

                    if (morphTargets[i].hasNormals != 0)
                    {
                        for (int j = 0; j < numVertices; j++)
                        {
                            listBytes.AddRange(BitConverter.GetBytes(morphTargets[i].normals[j].X));
                            listBytes.AddRange(BitConverter.GetBytes(morphTargets[i].normals[j].Y));
                            listBytes.AddRange(BitConverter.GetBytes(morphTargets[i].normals[j].Z));
                        }
                    }
                }
            }
        }
    }
}
