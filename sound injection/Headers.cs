using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace sound_injection
{
    public class HeadersClass
    {
        public static void HeadersMethod()
        {
            MapPathGoto:
            Console.WriteLine("Map Path: ");                                        //tell user to write the map path
            Program.MapPath = Console.ReadLine();                                   //save user input as MapPath variable
            Console.WriteLine($"You Wrote: {Program.MapPath}");                     //output the MapPath variable to output
            Program.MapPath = Program.MapPath.Replace("\"", "").Replace("\'", "");  //sometimes drag&drop files have quotes placed around them, remove the quotes
            if (File.Exists(Program.MapPath))                                       //check the file actaully exists
                Console.WriteLine("Congrats the file exists");
            else
            {
                Console.WriteLine("Could not find the file");
                goto MapPathGoto;
            }



            //check headers (0x12C should be "11081.07.04.30.0934.main")
            using (FileStream MapFileFileStream = new FileStream(Program.MapPath, FileMode.Open))   //open map file temporarily with using
            {
                BinaryReader MapFileBinaryReader = new BinaryReader(MapFileFileStream);             //open binary reader
                MapFileBinaryReader.BaseStream.Seek(0x12C, SeekOrigin.Current);                     //skip to engine header
                byte[] MapEngineStringBytes = MapFileBinaryReader.ReadBytes(24);                    //capture engine header bytes
                string MapEngineString = System.Text.Encoding.UTF8.GetString(MapEngineStringBytes, 0, MapEngineStringBytes.Length);     //convert engine header bytes to string
                Console.WriteLine($"Map Engine: {MapEngineString}");                                //print datatype
                if (MapEngineString != "11081.07.04.30.0934.main")
                {
                    Console.WriteLine("map is not halo 2 vista map you dumb shit");
                    goto MapPathGoto;
                }
                Console.WriteLine("");
            }


            SoundPathGoto:
            Console.WriteLine("16-bit Xbox APCDM WAV Path: ");                              //tell user to write the path
            Program.SoundPath = Console.ReadLine();                                         //save user input as variable
            Console.WriteLine($"You Wrote: {Program.SoundPath}");                           //output the variable to output
            Program.SoundPath = Program.SoundPath.Replace("\"", "").Replace("\'", "");      //sometimes drag&drop files have quotes placed around them, remove the quotes
            if (File.Exists(Program.SoundPath))                                             //check the file actaully exists
                Console.WriteLine("Congrats the file exists");
            else
            {
                Console.WriteLine("Could not find the file");
                goto SoundPathGoto;
            }


            //check headers (chunk size and format)
            using (FileStream SoundFileFileStream = new FileStream(Program.SoundPath, FileMode.Open))   //open wav file temporarily with using
            {
                BinaryReader SoundFileBinaryReader = new BinaryReader(SoundFileFileStream);             //open binary reader
                SoundFileBinaryReader.BaseStream.Seek(0x10, SeekOrigin.Current);
                int SoundHeaderChunkSize = SoundFileBinaryReader.ReadInt32();                           //capture datatype
                Console.WriteLine($"Chunk Size: {SoundHeaderChunkSize}");                               //print datatype
                int SoundHeaderwFormatTag = SoundFileBinaryReader.ReadInt16();                          //capture datatype
                Console.WriteLine($"wFormatTag: {SoundHeaderwFormatTag}");                              //print datatype
                if (SoundHeaderChunkSize != 20 || SoundHeaderwFormatTag != 105)
                {
                    Console.WriteLine("wav is not Xbox ADPCM codec");
                    goto SoundPathGoto;
                }
                Console.WriteLine("");
            }
        }

    }
}
