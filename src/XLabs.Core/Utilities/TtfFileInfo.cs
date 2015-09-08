namespace XLabs.Utilities
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// TTF File Information
    /// </summary>
    public class TtfFileInfo
    {
        /// <summary>
        /// Creates <see cref="TtfFileInfo"/> from <see cref="System.IO.Stream"/>.
        /// </summary>
        /// <param name="stream">Stream to create the structure. Stream will be disposed after use.</param>
        /// <returns><see cref="TtfFileInfo"/> if successful, otherwise null.</returns>
        public static TtfFileInfo FromStream(Stream stream)
        {
            try
            {
                using (var reader = new BigEndianReader(stream))
                {
                    var ttfFile = new TtfFileInfo()
                    {
                        Version = reader.ReadInt32()
                    };

                    if (ttfFile.Version != 0x74727565 && ttfFile.Version != 0x00010000) return null;

                    ttfFile.TableCount = reader.ReadInt16();
                    reader.BaseStream.Seek(6, SeekOrigin.Current);

                    for (var tableIndex = 0; tableIndex < ttfFile.TableCount; tableIndex++)
                    {
                        var tag = reader.ReadInt32();
                        reader.BaseStream.Seek(4, SeekOrigin.Current);
                        var offset = reader.ReadInt32();
                        var length = reader.ReadInt32();

                        if (tag != 0x6E616D65) continue;

                        reader.BaseStream.Seek(offset, SeekOrigin.Begin);

                        var table = reader.ReadBytes(length);

                        var count = GetInt16(table, 2);
                        var stringOffset = GetInt16(table, 4);

                        for (var record = 0; record < count; record++)
                        {
                            var nameidOffset = record*12 + 6;
                            var platformId = GetInt16(table, nameidOffset);
                            var nameidValue = GetInt16(table, nameidOffset + 6);

                            if (nameidValue != 4 || platformId != 1) continue;

                            var nameLength = GetInt16(table, nameidOffset + 8);
                            var nameOffset = stringOffset + GetInt16(table, nameidOffset + 10);

                            if (nameOffset < 0 || nameOffset + nameLength >= table.Length) continue;

                            ttfFile.FontName = Encoding.UTF8.GetString(table, nameOffset, nameLength);
                            return ttfFile;
                        }
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        private TtfFileInfo()
        {
            
        }

        /// <summary>
        /// Gets the Font Name.
        /// </summary>
        public string FontName { get; private set; }


        /// <summary>
        /// Gets the file version.
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// Gets the count of tables in the file.
        /// </summary>
        public short TableCount { get; private set; }

        private static int GetInt16(IReadOnlyList<byte> array, int offset )
        {
            return (array[ offset ] & 0xFF) << 8 | (array[ offset + 1 ] & 0xFF);
        }
    }
}