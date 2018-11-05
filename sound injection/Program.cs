using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace sound_injection
{
    public class Program
    {
        public static string MapPath = "";
        public static string SoundPath = "";
        public static UInt32 UserSNDTotalRawSpace;
        public static bool? DefaultSNDRawOffset = null;                       //nullible bool
        public static UInt32 FirstRawOffset;

        static void Main(string[] args)                                       //INITIAL
        {
            Console.WriteLine("Halo 2 Vista Sound Injector by blackdimund");
            Console.WriteLine("");
            Console.WriteLine("USAGE: Follow instructions.");
            Console.WriteLine("");
            Console.WriteLine("Currently there is no map expansion. You will need to hijack a snd! tag.");
            Console.WriteLine("Choose a similar one to what you want (use music for music injection, etc) Note the snd! tags offset,");
            Console.WriteLine("then you can choose to either ovverwrite its current snd! raw, or specify a");
            Console.WriteLine("new offset to paste the new sound raw. Make sure whatever you inject isnt long enough to ovverwrite important data");
            Console.WriteLine("If you don't understand that, well then good luck to you sir.");
            Console.WriteLine("");

            HeadersClass.HeadersMethod();                                     //check map paths and headers

            Console.WriteLine("Enter sound tag offset(decimal) from .map");                                           //ask user for sound tag offset
            Int32 SoundTagOffset = Int32.Parse(Console.ReadLine());                                                   //              save tag offset


            do
            {
                Console.WriteLine("Do you want to give new sound raw offset? (y/n) (if n, will ovverwrite original snd! RAW)");
                var caseSwitchRaw = Console.ReadLine();                                                         //capture user input
                switch (caseSwitchRaw)                                                                          //switch for user input
                {
                    case "y":                                                                                   //user types y, then blah
                        DefaultSNDRawOffset = false;                                                            //user will set manual raw offset
                        Console.WriteLine("Enter new sound raw offset(decimal)");
                        FirstRawOffset = UInt32.Parse(Console.ReadLine());                                      //capture user's new raw offset
                        break;
                    case "n":                                                                                   //user types n, then blah
                        DefaultSNDRawOffset = true;
                        break;
                    default:
                        Console.WriteLine("bruh just choose one k");
                        break;
                }
            } while (!DefaultSNDRawOffset.HasValue);


            using (FileStream MapFileFileStream = new FileStream(MapPath, FileMode.Open))                   //open map file temporarily with using)
            {
                //find ugh! tag, get to it. first get all core layouts data for .map
                BinaryReader MapFileBinaryReader = new BinaryReader(MapFileFileStream);                     //open binary reader

                Structs.H2VLayoutsCoreStruct h2vlayoutscorestruct = new Structs.H2VLayoutsCoreStruct();     //new struct instance      
                
                MapFileBinaryReader.BaseStream.Seek(0x8, SeekOrigin.Begin);                             //skip to offset       
                h2vlayoutscorestruct.FileSize               = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"Core Layouts FileSize               :   {h2vlayoutscorestruct.FileSize                  }");    //save and print core layouts data
                MapFileBinaryReader.BaseStream.Seek(0x10, SeekOrigin.Begin);                            //skip to offset
                h2vlayoutscorestruct.MetaOffset             = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"Core Layouts MetaOffset             :   {h2vlayoutscorestruct.MetaOffset                }");    //save and print core layouts data
                h2vlayoutscorestruct.TagDataOffset          = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"Core Layouts TagDataOffset          :   {h2vlayoutscorestruct.TagDataOffset             }");    //save and print core layouts data
                h2vlayoutscorestruct.TagDataSize            = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"Core Layouts TagDataSize            :   {h2vlayoutscorestruct.TagDataSize               }");    //save and print core layouts data
                h2vlayoutscorestruct.MetaSize               = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"Core Layouts MetaSize               :   {h2vlayoutscorestruct.MetaSize                  }");    //save and print core layouts data
                h2vlayoutscorestruct.MetaOffsetMask         = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"Core Layouts MetaOffsetMask         :   {h2vlayoutscorestruct.MetaOffsetMask            }");    //save and print core layouts data
                MapFileBinaryReader.BaseStream.Seek(0x14C, SeekOrigin.Begin);                           //skip to offset
                h2vlayoutscorestruct.Type                   = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"Core Layouts Type                   :   {h2vlayoutscorestruct.Type                      }");    //save and print core layouts data
                MapFileBinaryReader.BaseStream.Seek(0x16C, SeekOrigin.Begin);                           //skip to offset
                h2vlayoutscorestruct.StringBlockOffset      = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"Core Layouts StringBlockOffset      :   {h2vlayoutscorestruct.StringBlockOffset         }");    //save and print core layouts data
                h2vlayoutscorestruct.StringTableCount       = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"Core Layouts StringTableCount       :   {h2vlayoutscorestruct.StringTableCount          }");    //save and print core layouts data
                h2vlayoutscorestruct.StringTableSize        = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"Core Layouts StringTableSize        :   {h2vlayoutscorestruct.StringTableSize           }");    //save and print core layouts data
                h2vlayoutscorestruct.StringIndexTableOffset = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"Core Layouts StringIndexTableOffset :   {h2vlayoutscorestruct.StringIndexTableOffset    }");    //save and print core layouts data
                h2vlayoutscorestruct.StringTableOffset      = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"Core Layouts StringTableOffset      :   {h2vlayoutscorestruct.StringTableOffset         }");    //save and print core layouts data
                MapFileBinaryReader.BaseStream.Seek(0x1A4, SeekOrigin.Begin);                           //skip to offset
                h2vlayoutscorestruct.InternalName           = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"Core Layouts InternalName           :   {h2vlayoutscorestruct.InternalName              }");    //save and print core layouts data (doesnt grab text correctly)
                h2vlayoutscorestruct.ScenarioName           = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"Core Layouts ScenarioName           :   {h2vlayoutscorestruct.ScenarioName              }");    //save and print core layouts data (doesnt grab text correctly)
                MapFileBinaryReader.BaseStream.Seek(0x2CC, SeekOrigin.Begin);                           //skip to offset
                h2vlayoutscorestruct.FileTableCount         = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"Core Layouts FileTableCount         :   {h2vlayoutscorestruct.FileTableCount            }");    //save and print core layouts data
                h2vlayoutscorestruct.FileTableOffset        = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"Core Layouts FileTableOffset        :   {h2vlayoutscorestruct.FileTableOffset           }");    //save and print core layouts data
                h2vlayoutscorestruct.FileTableSize          = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"Core Layouts FileTableSize          :   {h2vlayoutscorestruct.FileTableSize             }");    //save and print core layouts data
                h2vlayoutscorestruct.FileIndexTableOffset   = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"Core Layouts FileIndexTableOffset   :   {h2vlayoutscorestruct.FileIndexTableOffset      }");    //save and print core layouts data
                MapFileBinaryReader.BaseStream.Seek(0x2E8, SeekOrigin.Begin);                           //skip to offset
                h2vlayoutscorestruct.RawTableOffset         = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"Core Layouts RawTableOffset         :   {h2vlayoutscorestruct.RawTableOffset            }");    //save and print core layouts data
                h2vlayoutscorestruct.RawTableSize           = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"Core Layouts RawTableSize           :   {h2vlayoutscorestruct.RawTableSize              }");    //save and print core layouts data
                h2vlayoutscorestruct.Checksum               = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"Core Layouts Checksum               :   {h2vlayoutscorestruct.Checksum                  }");    //save and print core layouts data
                Console.WriteLine("");

                Structs.SndStruct sndstruct = new Structs.SndStruct();                                                    //new sound tag struct instance
                MapFileBinaryReader.BaseStream.Seek(SoundTagOffset, SeekOrigin.Begin);                                    //skip to sound tag offset in .map
                sndstruct.Flags                     = MapFileBinaryReader.ReadInt16();      Console.WriteLine($"SND! Flags                          :   {sndstruct.Flags                      }");    //save and print SND! tag data
                sndstruct.SoundClass                = MapFileBinaryReader.ReadSByte();      Console.WriteLine($"SND! SoundClass                     :   {sndstruct.SoundClass                 }");    //save and print SND! tag data
                sndstruct.SampleRate                = MapFileBinaryReader.ReadSByte();      Console.WriteLine($"SND! SampleRate                     :   {sndstruct.SampleRate                 }");    //save and print SND! tag data
                sndstruct.Encoding                  = MapFileBinaryReader.ReadSByte();      Console.WriteLine($"SND! Encoding                       :   {sndstruct.Encoding                   }");    //save and print SND! tag data
                sndstruct.Compression               = MapFileBinaryReader.ReadSByte();      Console.WriteLine($"SND! Compression                    :   {sndstruct.Compression                }");    //save and print SND! tag data
                sndstruct.PlaybackParameterIndex    = MapFileBinaryReader.ReadInt16();      Console.WriteLine($"SND! PlaybackParameterIndex         :   {sndstruct.PlaybackParameterIndex     }");    //save and print SND! tag data
                sndstruct.PitchRangeIndex           = MapFileBinaryReader.ReadInt16();      Console.WriteLine($"SND! PitchRangeIndex                :   {sndstruct.PitchRangeIndex            }");    //save and print SND! tag data
                sndstruct.Unknown                   = MapFileBinaryReader.ReadSByte();      Console.WriteLine($"SND! Unknown                        :   {sndstruct.Unknown                    }");    //save and print SND! tag data
                sndstruct.ScaleIndex                = MapFileBinaryReader.ReadSByte();      Console.WriteLine($"SND! ScaleIndex                     :   {sndstruct.ScaleIndex                 }");    //save and print SND! tag data
                sndstruct.PromotionIndex            = MapFileBinaryReader.ReadSByte();      Console.WriteLine($"SND! PromotionIndex                 :   {sndstruct.PromotionIndex             }");    //save and print SND! tag data
                sndstruct.CustomPlaybackIndex       = MapFileBinaryReader.ReadSByte();      Console.WriteLine($"SND! CustomPlaybackIndex            :   {sndstruct.CustomPlaybackIndex        }");    //save and print SND! tag data
                sndstruct.ExtraInfoIndex            = MapFileBinaryReader.ReadInt16();      Console.WriteLine($"SND! ExtraInfoIndex                 :   {sndstruct.ExtraInfoIndex             }");    //save and print SND! tag data
                sndstruct.MaximumPlayTime           = MapFileBinaryReader.ReadInt32();      Console.WriteLine($"SND! MaximumPlayTime                :   {sndstruct.MaximumPlayTime            }");    //save and print SND! tag data
                Console.WriteLine("");

                //point reader to where tag filetable starts
                MapFileBinaryReader.BaseStream.Seek(h2vlayoutscorestruct.MetaOffset + 8, SeekOrigin.Begin);     //skip to offset in .map
                var MetaDataNonsenseLength = MapFileBinaryReader.ReadUInt32();                                  //skip nonsense data
                MapFileBinaryReader.BaseStream.Seek(h2vlayoutscorestruct.MetaOffset + MetaDataNonsenseLength, SeekOrigin.Begin);     //skip to offset in .map
                                                                                                                                     //list all tag filetables and find the UGH!
                Structs.FileTableStruct[] filetable = new Structs.FileTableStruct[h2vlayoutscorestruct.FileTableCount];         //define struct array
                UInt32 ughtagmemref = 0;                                                                        //create variable, to get ready to save data from for and if loop
                for (int i = 0; i < h2vlayoutscorestruct.FileTableCount; i++)                                   //for loop for struct array
                {
                    filetable[i].TagTypeBytes   = MapFileBinaryReader.ReadBytes(4);                           //defining data in struct array
                    filetable[i].TagTypeString  = System.Text.Encoding.UTF8.GetString(filetable[i].TagTypeBytes, 0, filetable[i].TagTypeBytes.Length);     //convert engine header bytes to string
                    filetable[i].TagDatumIndex  = MapFileBinaryReader.ReadUInt32();                           //defining data in struct array
                    filetable[i].TagMemRef      = MapFileBinaryReader.ReadUInt32();                           //defining data in struct array
                    filetable[i].TagSize        = MapFileBinaryReader.ReadUInt32();                           //defining data in struct array
                    if (filetable[i].TagTypeString == "!hgu")                                                   //when the UGH! file table is found
                        ughtagmemref = filetable[i].TagMemRef;                                                  //save the UGH! TagMemRef as new variable
                }
                //point reader to UGH! tag
                Console.WriteLine($"UGH! file table tagmemref: {ughtagmemref}");                                //print the UGH! TagMemRef variable
                MapFileBinaryReader.BaseStream.Seek((h2vlayoutscorestruct.MetaOffset + ughtagmemref), SeekOrigin.Begin);     //skip to offset in .map
                Structs.UghStruct ugh = new Structs.UghStruct();
                ugh.PlaybackParametersCount         = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"PlaybackParametersCount             : {ugh.PlaybackParametersCount         }");     //save and print tag data
                ugh.PlaybackParametersMemAddr       = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"PlaybackParametersMemAddr           : {ugh.PlaybackParametersMemAddr       }");     //save and print tag data
                ugh.ScalesCount                     = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"ScalesCount                         : {ugh.ScalesCount                     }");     //save and print tag data
                ugh.ScalesMemAddr                   = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"ScalesMemAddr                       : {ugh.ScalesMemAddr                   }");     //save and print tag data
                ugh.ImportNamesCount                = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"ImportNamesCount                    : {ugh.ImportNamesCount                }");     //save and print tag data
                ugh.ImportNamesMemAddr              = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"ImportNamesMemAddr                  : {ugh.ImportNamesMemAddr              }");     //save and print tag data
                ugh.PitchRangeParametersCount       = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"PitchRangeParametersCount           : {ugh.PitchRangeParametersCount       }");     //save and print tag data
                ugh.PitchRangeParametersMemAddr     = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"PitchRangeParametersMemAddr         : {ugh.PitchRangeParametersMemAddr     }");     //save and print tag data
                ugh.PitchRangesCount                = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"PitchRangesCount                    : {ugh.PitchRangesCount                }");     //save and print tag data
                ugh.PitchRangesMemAddr              = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"PitchRangesMemAddr                  : {ugh.PitchRangesMemAddr              }");     //save and print tag data
                ugh.PermutationsCount               = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"PermutationsCount                   : {ugh.PermutationsCount               }");     //save and print tag data
                ugh.PermutationsMemAddr             = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"PermutationsMemAddr                 : {ugh.PermutationsMemAddr             }");     //save and print tag data
                ugh.CustomPlaybacksCount            = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"CustomPlaybacksCount                : {ugh.CustomPlaybacksCount            }");     //save and print tag data
                ugh.CustomPlaybacksMemAddr          = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"CustomPlaybacksMemAddr              : {ugh.CustomPlaybacksMemAddr          }");     //save and print tag data
                ugh.RuntimePermutationFlagsCount    = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"RuntimePermutationFlagsCount        : {ugh.RuntimePermutationFlagsCount    }");     //save and print tag data
                ugh.RuntimePermutationFlagsMemAddr  = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"RuntimePermutationFlagsMemAddr      : {ugh.RuntimePermutationFlagsMemAddr  }");     //save and print tag data
                ugh.PermutationChunksCount          = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"PermutationChunksCount              : {ugh.PermutationChunksCount          }");     //save and print tag data
                ugh.PermutationChunksMemAddr        = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"PermutationChunksMemAddr            : {ugh.PermutationChunksMemAddr        }");     //save and print tag data
                ugh.PromotionsCount                 = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"PromotionsCount                     : {ugh.PromotionsCount                 }");     //save and print tag data
                ugh.PromotionsMemAddr               = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"PromotionsMemAddr                   : {ugh.PromotionsMemAddr               }");     //save and print tag data
                ugh.ExtraInfoCount                  = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"ExtraInfoCount                      : {ugh.ExtraInfoCount                  }");     //save and print tag data
                ugh.ExtraInfoMemAddr                = MapFileBinaryReader.ReadUInt32();     Console.WriteLine($"ExtraInfoMemAddr                    : {ugh.ExtraInfoMemAddr                }");     //save and print tag data
                Console.WriteLine("");
                ///////////////Pitch Ranges////////////
                MapFileBinaryReader.BaseStream.Seek((h2vlayoutscorestruct.MetaOffset + ugh.PitchRangesMemAddr), SeekOrigin.Begin);     //skip to offset
                Structs.PitchRangesStruct[] pitchranges = new Structs.PitchRangesStruct[ugh.PitchRangesCount];                          //struct array
                for (int i = 0; i < ugh.PitchRangesCount; i++)                                                                          //for loop for struct array
                {
                    pitchranges[i].ImportNameIndex                  = MapFileBinaryReader.ReadInt16();  //save tag data
                    pitchranges[i].PitchRangeParameterIndex         = MapFileBinaryReader.ReadInt16();  //save tag data
                    pitchranges[i].EncodedPermutationDataIndex      = MapFileBinaryReader.ReadInt16();  //save tag data
                    pitchranges[i].EncodedRuntimePermutationFlags   = MapFileBinaryReader.ReadInt16();  //save tag data
                    pitchranges[i].FirstPermutation                 = MapFileBinaryReader.ReadInt16();  //save tag data
                    pitchranges[i].PermutationCount                 = MapFileBinaryReader.ReadInt16();  //save tag data
                }
                Console.WriteLine($"Selected SND pitch range block index {sndstruct.PitchRangeIndex}:");
                Console.WriteLine($"[{sndstruct.PitchRangeIndex}]ImportNameIndex                     : {pitchranges[sndstruct.PitchRangeIndex].ImportNameIndex                  }");
                Console.WriteLine($"[{sndstruct.PitchRangeIndex}]PitchRangeParameterIndex            : {pitchranges[sndstruct.PitchRangeIndex].PitchRangeParameterIndex         }");
                Console.WriteLine($"[{sndstruct.PitchRangeIndex}]EncodedPermutationDataIndex         : {pitchranges[sndstruct.PitchRangeIndex].EncodedPermutationDataIndex      }");
                Console.WriteLine($"[{sndstruct.PitchRangeIndex}]EncodedRuntimePermutationFlags      : {pitchranges[sndstruct.PitchRangeIndex].EncodedRuntimePermutationFlags   }");
                Console.WriteLine($"[{sndstruct.PitchRangeIndex}]FirstPermutation                    : {pitchranges[sndstruct.PitchRangeIndex].FirstPermutation                 }");
                Console.WriteLine($"[{sndstruct.PitchRangeIndex}]PermutationCount                    : {pitchranges[sndstruct.PitchRangeIndex].PermutationCount                 }");
                ///////////////Permutations////////////
                var permutationsoffset = (h2vlayoutscorestruct.MetaOffset + ugh.PermutationsMemAddr);
                MapFileBinaryReader.BaseStream.Seek(permutationsoffset, SeekOrigin.Begin);                              //skip to offset
                Structs.PermutationsStruct[] permutations = new Structs.PermutationsStruct[ugh.PermutationsCount];      //struct array
                for (int i = 0; i < ugh.PermutationsCount; i++)                                                         //for loop for struct array
                {
                    permutations[i].ImportNameIndex         = MapFileBinaryReader.ReadInt16();
                    permutations[i].EncodedSkipFraction     = MapFileBinaryReader.ReadInt16();
                    permutations[i].Gain                    = MapFileBinaryReader.ReadByte();
                    permutations[i].PermutationInfoIndex    = MapFileBinaryReader.ReadByte();
                    permutations[i].LanguageNeutralTime     = MapFileBinaryReader.ReadInt16();
                    permutations[i].SampleSize              = MapFileBinaryReader.ReadUInt32();
                    permutations[i].FirstChunk              = MapFileBinaryReader.ReadInt16();
                    permutations[i].ChunkCount              = MapFileBinaryReader.ReadInt16();
                }
                var UserSNDPermutationsIndex = pitchranges[sndstruct.PitchRangeIndex].FirstPermutation;     //save user's SND Permutations index as new variable to be less confusing
                Console.WriteLine($"Selected SND Permutations block index {UserSNDPermutationsIndex}:");
                Console.WriteLine($"[{UserSNDPermutationsIndex}]ImportNameIndex                     : {permutations[UserSNDPermutationsIndex].ImportNameIndex       }");
                Console.WriteLine($"[{UserSNDPermutationsIndex}]EncodedSkipFraction                 : {permutations[UserSNDPermutationsIndex].EncodedSkipFraction   }");
                Console.WriteLine($"[{UserSNDPermutationsIndex}]Gain                                : {permutations[UserSNDPermutationsIndex].Gain                  }");
                Console.WriteLine($"[{UserSNDPermutationsIndex}]PermutationInfoIndex                : {permutations[UserSNDPermutationsIndex].PermutationInfoIndex  }");
                Console.WriteLine($"[{UserSNDPermutationsIndex}]LanguageNeutralTime                 : {permutations[UserSNDPermutationsIndex].LanguageNeutralTime   }");
                Console.WriteLine($"[{UserSNDPermutationsIndex}]SampleSize                          : {permutations[UserSNDPermutationsIndex].SampleSize            }");
                Console.WriteLine($"[{UserSNDPermutationsIndex}]FirstChunk                          : {permutations[UserSNDPermutationsIndex].FirstChunk            }");
                Console.WriteLine($"[{UserSNDPermutationsIndex}]ChunkCount                          : {permutations[UserSNDPermutationsIndex].ChunkCount            }");
                ///////////////Permutation Chunks////////////
                var permutationchunksoffset = h2vlayoutscorestruct.MetaOffset + ugh.PermutationChunksMemAddr;
                MapFileBinaryReader.BaseStream.Seek((permutationchunksoffset), SeekOrigin.Begin);                                           //skip to offset
                Structs.PermutationChunksStruct[] permutationchunks = new Structs.PermutationChunksStruct[ugh.PermutationChunksCount];      //struct array
                for (int i = 0; i < ugh.PermutationChunksCount; i++)                                                                        //for loop for struct array
                {
                    permutationchunks[i].FileOffset     = MapFileBinaryReader.ReadUInt32();
                    permutationchunks[i].ChunkSize      = MapFileBinaryReader.ReadUInt16();
                    permutationchunks[i].Unknown0       = MapFileBinaryReader.ReadSByte();
                    permutationchunks[i].Unknown1       = MapFileBinaryReader.ReadSByte();
                    permutationchunks[i].RuntimeIndex   = MapFileBinaryReader.ReadInt32();
                }
                var UserSNDFirstPermutationChunkIndex = permutations[UserSNDPermutationsIndex].FirstChunk;     //save user's SND First Permutation Chunk index as new variable to be less confusing
                Console.WriteLine($"Selected SND Permutation Chunk block index {UserSNDFirstPermutationChunkIndex}:");
                Console.WriteLine($"[{UserSNDFirstPermutationChunkIndex}]FileOffset                  : {permutationchunks[UserSNDFirstPermutationChunkIndex].FileOffset     }");
                Console.WriteLine($"[{UserSNDFirstPermutationChunkIndex}]ChunkSize                   : {permutationchunks[UserSNDFirstPermutationChunkIndex].ChunkSize      }");
                Console.WriteLine($"[{UserSNDFirstPermutationChunkIndex}]Unknown0                    : {permutationchunks[UserSNDFirstPermutationChunkIndex].Unknown0       }");
                Console.WriteLine($"[{UserSNDFirstPermutationChunkIndex}]Unknown1                    : {permutationchunks[UserSNDFirstPermutationChunkIndex].Unknown1       }");
                Console.WriteLine($"[{UserSNDFirstPermutationChunkIndex}]RuntimeIndex                : {permutationchunks[UserSNDFirstPermutationChunkIndex].RuntimeIndex   }");

                //calculate how much raw space the SND! actually has (will be slightly higher than sample size due to unused null data between block)
                var UserSNDLastPermutationChunk = UserSNDFirstPermutationChunkIndex + permutations[UserSNDPermutationsIndex].ChunkCount;
                UserSNDTotalRawSpace = ((permutationchunks[UserSNDLastPermutationChunk - 1].FileOffset) + permutationchunks[UserSNDLastPermutationChunk - 1].ChunkSize) - (permutationchunks[UserSNDFirstPermutationChunkIndex].FileOffset)  ; //final - initial
                Console.WriteLine($"UserSNDTotalRawSpace: {UserSNDTotalRawSpace}");

                uint newwavchunkremainder, NewWavTotalRawSpace;                                             //declare variables
                Int16 newwavchunkscount;                                                                    //declare variables
                List<Structs.NewWavChunks> newwavchunks = new List<Structs.NewWavChunks>();                 //struct list initialized (empty)

                //Calculate how much data will be needed (blocks of 65520 bytes) from wav file
                using (FileStream SoundFileFileStream = new FileStream(SoundPath, FileMode.Open))           //open Sound file temporarily with using)
                {
                    //opening wav filestream
                    BinaryReader SoundFileBinaryReader = new BinaryReader(SoundFileFileStream);             //open binary reader
                    SoundFileBinaryReader.BaseStream.Seek(0x30, SeekOrigin.Begin);                          //skip wav headers
                    NewWavTotalRawSpace = Convert.ToUInt32(SoundFileFileStream.Length) - 0x30;              //capture total file length minus headers
                    newwavchunkscount = Convert.ToInt16(NewWavTotalRawSpace / 65520);                       //calculate how many chunks for new wav
                    for (int i = 0; i < newwavchunkscount; i++)                                             //for loop for struct array
                        newwavchunks.Add(new Structs.NewWavChunks( SoundFileBinaryReader.ReadBytes(65520)));     //capture sound chunks of 65520 bytes
                    newwavchunkremainder = NewWavTotalRawSpace - (Convert.ToUInt32(newwavchunkscount * 65520));  //calculate remainder
                    newwavchunks.Add(new Structs.NewWavChunks(SoundFileBinaryReader.ReadBytes(Convert.ToInt32(newwavchunkremainder))));  //read remainder
                    Console.WriteLine($"NewWavTotalRawSpace : {NewWavTotalRawSpace}");
                    Console.WriteLine($"newwavchunkscount   : {newwavchunkscount}");
                    Console.WriteLine($"newwavchunkremainder: {newwavchunkremainder}");
                }   //end soundfilestream

                BinaryWriter MapFileBinaryWriter = new BinaryWriter(MapFileFileStream);                     //open binary writer
                MapFileBinaryWriter.BaseStream.Seek(SoundTagOffset + 0x10, SeekOrigin.Begin);               //skip to SND! maximum play time offset
                MapFileBinaryWriter.Write(999999);                                                          //set SND! maximum play time very high
                var UserSNDPermutationsOffset = permutationsoffset + (16 * UserSNDPermutationsIndex) + 0x8; //skip to (Permutations) + (UserSND Permutation) + skipToSampleSize
                MapFileBinaryWriter.BaseStream.Seek(UserSNDPermutationsOffset, SeekOrigin.Begin);           //skip to SND Sample Size
                MapFileBinaryWriter.Write(NewWavTotalRawSpace);                                             //write new Sample Size
                MapFileBinaryWriter.BaseStream.Seek(0x2, SeekOrigin.Current);                               //skip to SND Chunk Count
                MapFileBinaryWriter.Write(newwavchunkscount + 1);                                           //write new Chunk Count +1 for remainder
                
                var UserSNDPermutationChunksOffset = permutationchunksoffset + (12 * UserSNDFirstPermutationChunkIndex);    //find first SND! permutation chunk offset
                MapFileBinaryWriter.BaseStream.Seek(UserSNDPermutationChunksOffset, SeekOrigin.Begin);      //skip to (Permutation Chunks) + (UserSND First Permutation Chunk)
                
                //if user indicated a new raw offset
                if (DefaultSNDRawOffset == false)                                                       //if user indicated a new raw offset
                {
                    var RawOffset = FirstRawOffset;                                                     //set pointers RawOffset to first raw offset
                    for (int i = 0; i < newwavchunkscount; i++)                                         //loop for all new permutation chunks blocks
                    {
                        MapFileBinaryWriter.Write(RawOffset);                                           //write new File Offset (user given raw offset) + (i starting at 0)*(rawchunksize)
                        MapFileBinaryWriter.Write(Convert.ToUInt16(65520));                             //write new chunk size
                        MapFileBinaryWriter.Write(Convert.ToSByte(0));                                  //write Unknown
                        MapFileBinaryWriter.Write(Convert.ToSByte(0));                                  //write Unknown
                        MapFileBinaryWriter.Write(Convert.ToInt32(-1));                                 //write Runtime Index
                        RawOffset += 65520;
                    }
                    MapFileBinaryWriter.Write(RawOffset);                                               //remainder chunk file offset
                    MapFileBinaryWriter.Write(Convert.ToUInt16(newwavchunkremainder));                  //remainder chunk chunk size
                    MapFileBinaryWriter.Write(Convert.ToSByte(0));                                      //write Unknown
                    MapFileBinaryWriter.Write(Convert.ToSByte(0));                                      //write Unknown
                    MapFileBinaryWriter.Write(Convert.ToInt32(-1));                                     //write Runtime Index
                }
                //if user indicated no new offset(default)
                if (DefaultSNDRawOffset == true)                                                        //if user indicated no new offset(default)
                {
                    FirstRawOffset = MapFileBinaryReader.ReadUInt32();                                  //caputure first file offset
                    MapFileBinaryWriter.BaseStream.Seek(-0x4, SeekOrigin.Current);                      //put pointer back to file offset
                    var RawOffset = FirstRawOffset;                                                     //set pointers RawOffset to first raw offset
                    for (int i = 0; i < newwavchunkscount; i++)                                         //loop for all new permutation chunks blocks
                    {
                        MapFileBinaryWriter.Write(RawOffset);                                           //write new File Offset (user given raw offset) + (i starting at 0)*(rawchunksize)
                        MapFileBinaryWriter.Write(Convert.ToUInt16(65520));                             //write new chunk size
                        MapFileBinaryWriter.Write(Convert.ToSByte(0));                                  //write Unknown
                        MapFileBinaryWriter.Write(Convert.ToSByte(0));                                  //write Unknown
                        MapFileBinaryWriter.Write(Convert.ToInt32(-1));                                 //write Runtime Index
                        RawOffset += 65520;
                    }
                    MapFileBinaryWriter.Write(RawOffset);                                               //remainder chunk file offset
                    MapFileBinaryWriter.Write(Convert.ToUInt16(newwavchunkremainder));                  //remainder chunk chunk size
                    MapFileBinaryWriter.Write(Convert.ToSByte(0));                                      //write Unknown
                    MapFileBinaryWriter.Write(Convert.ToSByte(0));                                      //write Unknown
                    MapFileBinaryWriter.Write(Convert.ToInt32(-1));                                     //write Runtime Index
                }

                //now start writing the new wav RAW to .map, FirstRawOffset should be offset needed. make list of structs to map out old data
                MapFileBinaryWriter.BaseStream.Seek(FirstRawOffset, SeekOrigin.Begin);                  //skip to first new raw offset
                for (int i = 0; i < newwavchunkscount+1; i++)                                           //loop for all new permutation chunks blocks (+1 for remainder)
                    MapFileBinaryWriter.Write(newwavchunks[i].WavBytes);                                //paste all newwavchunks blocks
                
                


            }   //end mapfilestream



















            Console.Read();                                                                             //pause
        }


    }

}
