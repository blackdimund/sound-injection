using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sound_injection
{
    class Structs
    {
        public struct SndStruct
        {
            public Int16 Flags;
            public SByte SoundClass;
            public SByte SampleRate;
            public SByte Encoding;
            public SByte Compression;
            public Int16 PlaybackParameterIndex;
            public Int16 PitchRangeIndex;
            public SByte Unknown;
            public SByte ScaleIndex;
            public SByte PromotionIndex;
            public SByte CustomPlaybackIndex;
            public Int16 ExtraInfoIndex;
            public Int32 MaximumPlayTime;
        }

        public struct H2VLayoutsCoreStruct
        {
            public UInt32 FileSize;
            public UInt32 MetaOffset;
            public UInt32 TagDataOffset;
            public UInt32 TagDataSize;
            public UInt32 MetaSize;
            public UInt32 MetaOffsetMask;

            public UInt32 Type;

            public UInt32 StringBlockOffset;
            public UInt32 StringTableCount;
            public UInt32 StringTableSize;
            public UInt32 StringIndexTableOffset;
            public UInt32 StringTableOffset;

            public UInt32 InternalName;
            public UInt32 ScenarioName;

            public UInt32 FileTableCount;
            public UInt32 FileTableOffset;
            public UInt32 FileTableSize;
            public UInt32 FileIndexTableOffset;

            public UInt32 RawTableOffset;
            public UInt32 RawTableSize;

            public UInt32 Checksum;
        }

        public struct FileTableStruct
        {
            public byte[] TagTypeBytes;
            public string TagTypeString;
            public UInt32 TagDatumIndex;
            public UInt32 TagMemRef;
            public UInt32 TagSize;
        }

        public struct UghStruct
        {
            public UInt32 PlaybackParametersCount;
            public UInt32 PlaybackParametersMemAddr;
            public UInt32 ScalesCount;
            public UInt32 ScalesMemAddr;
            public UInt32 ImportNamesCount;
            public UInt32 ImportNamesMemAddr;
            public UInt32 PitchRangeParametersCount;
            public UInt32 PitchRangeParametersMemAddr;
            public UInt32 PitchRangesCount;
            public UInt32 PitchRangesMemAddr;
            public UInt32 PermutationsCount;
            public UInt32 PermutationsMemAddr;
            public UInt32 CustomPlaybacksCount;
            public UInt32 CustomPlaybacksMemAddr;
            public UInt32 RuntimePermutationFlagsCount;
            public UInt32 RuntimePermutationFlagsMemAddr;
            public UInt32 PermutationChunksCount;
            public UInt32 PermutationChunksMemAddr;
            public UInt32 PromotionsCount;
            public UInt32 PromotionsMemAddr;
            public UInt32 ExtraInfoCount;
            public UInt32 ExtraInfoMemAddr;
        }

        public struct PitchRangesStruct
        {
            public Int16 ImportNameIndex;
            public Int16 PitchRangeParameterIndex;
            public Int16 EncodedPermutationDataIndex;
            public Int16 EncodedRuntimePermutationFlags;
            public Int16 FirstPermutation;
            public Int16 PermutationCount;
        }

        public struct PermutationsStruct
        {
            public Int16 ImportNameIndex;
            public Int16 EncodedSkipFraction;
            public Byte Gain;
            public Byte PermutationInfoIndex;
            public Int16 LanguageNeutralTime;
            public UInt32 SampleSize;
            public Int16 FirstChunk;
            public Int16 ChunkCount;
        }

        public struct PermutationChunksStruct
        {
            public UInt32 FileOffset;
            public UInt16 ChunkSize;
            public SByte Unknown0;
            public SByte Unknown1;
            public Int32 RuntimeIndex;
        }
        public struct NewWavChunks
        {
            public byte[] WavBytes;
            public NewWavChunks(byte[] wavChunks)       //have to apparently make this to a contructor so i can make a list of this struct in loop
            {
                WavBytes = wavChunks;
            }
        }
    }
}
