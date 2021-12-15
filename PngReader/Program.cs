using System;
using System.IO;
using System.Linq;
using System.Text;


var fileName = args[0];
using var f = File.OpenRead(fileName);
var signature = new byte[8];
var signatureLen = f.Read(signature);
var pngSignature = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, };
if (signatureLen != 8 || ! Enumerable.SequenceEqual(signature, pngSignature))
    throw new ArgumentException("Not a PNG file");

var cl = new byte[4];
f.Read(cl);
var chunkLength = cl[0] * 0x1000000 + cl[1] * 0x10000 + cl[2] * 0x100 + cl[3];

var chunkIdArray = new byte[4];
f.Read(chunkIdArray);
var chunkId = Encoding.ASCII.GetString(chunkIdArray);
if (chunkId != "IHDR")
    throw new ArgumentException("IHDR must be the first chunk");

var ihdr = new byte[chunkLength];
f.Read(ihdr);
var crc = new byte[4];
f.Read(crc);

var width = ihdr[0] * 0x1000000 + ihdr[1] * 0x10000 + ihdr[2] * 0x100 + ihdr[3];
var height = ihdr[4] * 0x1000000 + ihdr[5] * 0x10000 + ihdr[6] * 0x100 + ihdr[7];
Console.WriteLine($"Image is {width} x {height} pixels");
