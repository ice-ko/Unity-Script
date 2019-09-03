using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_AnimationSystem {

    /*
     * Animation Save System
     * */
    public static class SaveSystem {


        public struct HeaderData {

            private static string[] prevVersions = new[] { "0.01" };
            private const string version = "1.00";
            private const string versionDate = "12-05-2018";
            private static byte versionByte = (byte)prevVersions.Length;


            public int saveByteLength;
            public string animationVersion;
            public string headerDataVersion;
            public string date;


            public static HeaderData Generate(string saveContent) {
                HeaderData headerData;
                headerData.saveByteLength = System.Text.Encoding.UTF8.GetBytes(saveContent).Length;
                headerData.animationVersion = V_Animation.version;
                headerData.headerDataVersion = version;
                headerData.date = System.DateTime.Now.ToString("dd-MM-yy_HH-mm-ss");
                return headerData;
            }
            public static string Save(string saveContent) {
                return Save(Generate(saveContent));
            }
            public static string Save(HeaderData headerData) {
                string[] content = new string[]{
                headerData.saveByteLength+"",
                headerData.animationVersion,
                headerData.date,
                headerData.headerDataVersion,
            };
                return string.Join("#HEADER#", content);
            }
            public static HeaderData Load(string header) {
                string[] content = V_Animation.SplitString(header, "#HEADER#");
                HeaderData headerData;
                headerData.saveByteLength = int.Parse(content[0]);
                headerData.animationVersion = content[1];
                headerData.date = content[2];
                headerData.headerDataVersion = content[3];
                return headerData;
            }

            public override string ToString() {
                return
                    "FILE HEADER" + "\n" +
                    "saveByteLength: " + saveByteLength + "\n" +
                    "gameVersion: " + animationVersion + "\n" +
                    "headerDataVersion: " + headerDataVersion + "\n" +
                    "date: " + date + "\n" +
                    "";
            }
        }


        public struct FileData {

            public enum FileType {
                Animation,
                AnimationType,
                UnitInfo,
                MapSpawns,
                GameState_Stats,
                NinjaTycoonSave,
            }

            public FileType fileType;
            public HeaderData headerData;
            public string save;

            public static FileData Load(byte[] byteArr) {
                FileType fileType = (FileType)byteArr[0];
                int fileTypeByteAmount = 1;
                byte[] headerSizeBytes = new byte[] { byteArr[1], byteArr[2] };
                int headerSizeByteAmount = headerSizeBytes.Length;
                short headerSize = System.BitConverter.ToInt16(headerSizeBytes, 0);

                byte[] headerBytes = new byte[headerSize];
                System.Array.Copy(byteArr, fileTypeByteAmount + headerSizeByteAmount, headerBytes, 0, headerSize);
                string headerCompressed = System.Text.Encoding.UTF8.GetString(headerBytes);
                string header = GetUnCompressedString(headerCompressed);

                HeaderData headerData = HeaderData.Load(header);

                byte[] saveBytes = new byte[headerData.saveByteLength];
                System.Array.Copy(byteArr, fileTypeByteAmount + headerSizeByteAmount + headerBytes.Length, saveBytes, 0, headerData.saveByteLength);
                string saveCompressed = System.Text.Encoding.UTF8.GetString(saveBytes);

                string save = GetUnCompressedString(saveCompressed);

                FileData fileData;
                fileData.fileType = fileType;
                fileData.headerData = headerData;
                fileData.save = save;

                return fileData;
            }

            public static byte[] Save(FileType fileType, string header, string save) {
                // Convert header into bytes
                byte fileTypeByte = (byte)fileType;
                byte[] headerBytes = System.Text.Encoding.UTF8.GetBytes(header);

                // Save 2 bytes for header length
                short headerSize = (short)headerBytes.Length;
                byte[] headerSizeBytes = System.BitConverter.GetBytes(headerSize);

                // Get save bytes
                byte[] saveBytes = System.Text.Encoding.UTF8.GetBytes(save);

                // Merge all together
                List<byte> totalBytes = new List<byte>();
                totalBytes.Add(fileTypeByte);
                totalBytes.AddRange(headerSizeBytes);
                totalBytes.AddRange(headerBytes);
                totalBytes.AddRange(saveBytes);

                return totalBytes.ToArray();
            }
        }









        public static string GetCompressedString(string save) {
            return System.Convert.ToBase64String(CLZF2.Compress(System.Text.Encoding.ASCII.GetBytes(save)));
        }
        public static string GetUnCompressedString(string save) {
            return System.Text.Encoding.ASCII.GetString(CLZF2.Decompress(System.Convert.FromBase64String(save)));
        }





        public static void Save(string folderPath, string saveName, FileData.FileType fileType, string saveString) {
            byte[] fileSaveBytes;
            Save(folderPath, saveName, fileType, saveString, out fileSaveBytes);
        }
        public static void Save(string folderPath, string saveName, FileData.FileType fileType, string saveString, out byte[] fileSaveBytes) {
            // Save name containing extension
            string saveFile = folderPath + saveName;
            Save(saveFile, fileType, saveString, out fileSaveBytes);
        }

        public static void Save(string fullSavePath, FileData.FileType fileType, string saveString, out byte[] fileSaveBytes) {
            // Save name containing extension
            string saveFile = fullSavePath;
            //Compress Save
            string saveStringCompressed = GetCompressedString(saveString);
            string header = GetCompressedString(Save_Header(saveStringCompressed));

            fileSaveBytes = FileData.Save(fileType, header, saveStringCompressed);

            File.WriteAllBytes(saveFile, fileSaveBytes);
        }

        public static bool Load(string folderPath, string file, out FileData fileData) {
            // Byte save
            return Load(folderPath + file, out fileData);
        }
        public static bool Load(string fullFilePath, out FileData fileData) {
            // File name containing extension
            // Assumes extension is 3 characters long
            byte[] readAllBytes = File.ReadAllBytes(fullFilePath);

            return Load(readAllBytes, out fileData);
        }
        public static bool Load(byte[] byteArr, out FileData fileData) {
            try {
                fileData = FileData.Load(byteArr);
                return true;
            } catch (Exception e) {
                Debug.Log("Load Failed: " + e);
                fileData = default(FileData);
                return false;
            }
        }



        private static string Save_Header(string saveContent) {
            //Returns a string to be used in savefiles
            return HeaderData.Save(saveContent);
        }
        private static HeaderData Load_Header(string header) {
            HeaderData headerData = HeaderData.Load(header);
            return headerData;
        }
    }

}