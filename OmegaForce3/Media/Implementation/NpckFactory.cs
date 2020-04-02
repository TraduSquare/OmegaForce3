﻿// -----------------------------------------------------------------------
// <copyright file="NpckFactory.cs" company="none">
// Copyright (C) 2014 
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by 
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful, 
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details. 
//
//   You should have received a copy of the GNU General Public License
//   along with this program.  If not, see "http://www.gnu.org/licenses/". 
// </copyright>
// <author>pleoNeX</author>
// <email>benito356@gmail.com</email>
// <date>06/07/2014</date>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ninoimager.ImageProcessing;
using Color     = Emgu.CV.Structure.Bgra;
using Bitmap = Emgu.CV.Image<Emgu.CV.Structure.Bgra, System.Byte>;

namespace Ninoimager.Format
{
	public class NpckFactory
	{
		public static Npck FromBackgroundImage(string image)
		{
			using (Bitmap emgu = new Bitmap(image))
				return FromBackgroundImage(emgu);
		}

		public static Npck FromBackgroundImage(Bitmap image)
		{
			MemoryStream nclrStr = new MemoryStream();
			MemoryStream ncgrStr = new MemoryStream();
			MemoryStream nscrStr = new MemoryStream();

			BackgroundImporter importer = new BackgroundImporter();
			importer.ImportBackground(image, nscrStr, ncgrStr, nclrStr);

			nclrStr.Position = ncgrStr.Position = nscrStr.Position = 0;
			return Npck.FromBackgroundStreams(nscrStr, ncgrStr, nclrStr);
		}

		public static Npck FromBackgroundImage(string image, Npck original,
			bool changePalette = false)
		{
			using (Bitmap emgu = new Bitmap(image))
				return FromBackgroundImage(emgu, original, changePalette);
		}

		public static Npck FromBackgroundImage(Bitmap image, Npck original, 
			bool changePalette = false)
		{
			if (original[0] == null || original[1] == null)
				throw new FormatException(
					"Can not import image.\n" + 
					"There is not palette or image in the original pack."
				);

			MemoryStream nclrStr = new MemoryStream();
			MemoryStream ncgrStr = new MemoryStream();
			MemoryStream nscrStr = new MemoryStream();

			// Import image
			BackgroundImporter importer = new BackgroundImporter();
			importer.SetOriginalSettings(original[2], original[1], original[0], changePalette);
			importer.ImportBackground(image, nscrStr, ncgrStr, nclrStr);

			nclrStr.Position = ncgrStr.Position = nscrStr.Position = 0;
			return Npck.FromBackgroundStreams(nscrStr, ncgrStr, nclrStr);
		}

		public static Npck[] FromBackgroundImageSharePalette(string[] images)
		{
			Bitmap[] emguImgs = new Bitmap[images.Length];
			for (int i = 0; i < images.Length; i++)
				emguImgs[i] = new Bitmap(images[i]);

			Npck[] npcks = FromBackgroundImageSharePalette(emguImgs, new BackgroundImporter());

			// Dispose images
			foreach (Bitmap emgu in emguImgs)
				emgu.Dispose();

			return npcks;
		}

		public static Npck[] FromBackgroundImageSharePalette(string[] images, Npck original)
		{
			Bitmap[] emguImgs = new Bitmap[images.Length];
			for (int i = 0; i < images.Length; i++)
				emguImgs[i] = new Bitmap(images[i]);

			BackgroundImporter importer = new BackgroundImporter();
			importer.SetOriginalSettings(original[6], original[1], original[0]);
			Npck[] npcks = FromBackgroundImageSharePalette(emguImgs, importer);

			// Dispose images
			foreach (Bitmap emgu in emguImgs)
				emgu.Dispose();

			return npcks;
		}

		public static Npck[] FromBackgroundImageSharePalette(Bitmap[] images, BackgroundImporter importer)
		{
			if (!(importer.Quantization is FixedPaletteQuantization)) {
				// Concatenate images
				Bitmap combinedImg = images[0].Clone();
				for (int i = 1; i < images.Length; i++)
					combinedImg = combinedImg.ConcateHorizontal(images[i]);

				NdsQuantization quantization = new NdsQuantization();
				quantization.Quantizate(combinedImg);
				importer.Quantization = new FixedPaletteQuantization(quantization.Palette);

				combinedImg.Dispose();
			}

			// Create packs
			Npck[] packs = new Npck[images.Length];
			for (int i = 0; i < images.Length; i++) {
				MemoryStream nclrStr = new MemoryStream();
				MemoryStream ncgrStr = new MemoryStream();
				MemoryStream nscrStr = new MemoryStream();

				importer.ImportBackground(images[i], nscrStr, ncgrStr, nclrStr);
				nclrStr.Position = ncgrStr.Position = nscrStr.Position = 0;

				// Only first pack file has palette file
				if (i == 0)
					packs[i] = Npck.FromBackgroundStreams(nscrStr, ncgrStr, nclrStr);
				else
					packs[i] = Npck.FromBackgroundStreams(nscrStr, ncgrStr, null);
			}

			return packs;
		}

		public static Npck[] FromBackgroundImageShareImage(string[] images)
		{
			Bitmap[] emguImgs = new Bitmap[images.Length];
			for (int i = 0; i < images.Length; i++)
				emguImgs[i] = new Bitmap(images[i]);

			Npck[] npcks = FromBackgroundImageShareImage(emguImgs, new BackgroundImporter());

			// Dispose images
			foreach (Bitmap emgu in emguImgs)
				emgu.Dispose();

			return npcks;
		}

		public static Npck[] FromBackgroundImageShareImage(string[] images, Npck original)
		{
			Bitmap[] emguImgs = new Bitmap[images.Length];
			for (int i = 0; i < images.Length; i++)
				emguImgs[i] = new Bitmap(images[i]);

			BackgroundImporter importer = new BackgroundImporter();
			importer.SetOriginalSettings(original[6], original[1], original[0]);
			Npck[] npcks = FromBackgroundImageShareImage(emguImgs, importer);

			// Dispose images
			foreach (Bitmap emgu in emguImgs)
				emgu.Dispose();

			return npcks;
		}

		public static Npck[] FromBackgroundImageShareImage(Bitmap[] images, BackgroundImporter importer)
		{
			Npck[] packs = new Npck[images.Length];

			Bitmap combinedImg = images[0].Clone();
			// Concatenate images
			for (int i = 1; i < images.Length; i++)
				combinedImg = combinedImg.ConcateHorizontal(images[i]);

			if (!(importer.Quantization is FixedPaletteQuantization)) {
				// Get quantization to share palette
				NdsQuantization quantization = new NdsQuantization();
				quantization.Quantizate(combinedImg);
				importer.Quantization = new FixedPaletteQuantization(quantization.Palette);
			}

			// Get the palette and image file that it's shared
			MemoryStream nclrStr = new MemoryStream();
			MemoryStream ncgrStr = new MemoryStream();
			importer.ImportBackground(combinedImg, null, ncgrStr, nclrStr);
			nclrStr.Position = ncgrStr.Position = 0;

			// Get the array of pixel from the image file
			Ncgr ncgr = new Ncgr(ncgrStr);
			Pixel[] fullImage = ncgr.GetPixels();
			ncgrStr.Position = 0;

			// Create packs
			for (int i = 0; i < images.Length; i++) {
				MemoryStream nscrStr = new MemoryStream();
				importer.ImportBackgroundShareImage(images[i], fullImage, nscrStr);
				nscrStr.Position = 0;

				// Only first pack file has palette and image files
				if (i == 0)
					packs[i] = Npck.FromBackgroundStreams(nscrStr, ncgrStr, nclrStr);
				else
					packs[i] = Npck.FromBackgroundStreams(nscrStr, null, null);
			}

			combinedImg.Dispose();
			return packs;
		}

		public static Npck[] FromBackgroundImageSharePaletteChangeDepth(string[] images,
			Npck original, bool partialImage = false)
		{
			Bitmap[] emguImgs = new Bitmap[images.Length];
			for (int i = 0; i < images.Length; i++)
				emguImgs[i] = new Bitmap(images[i]);

			BackgroundImporter importer = new BackgroundImporter();
			importer.SetOriginalSettings(original[6], original[1], original[0]);
			importer.PartialImage = partialImage;

			// Create packs
			Npck[] packs = new Npck[images.Length];
			for (int i = 0; i < images.Length; i++) {
				MemoryStream nclrStr = new MemoryStream();
				MemoryStream ncgrStr = new MemoryStream();
				MemoryStream nscrStr = new MemoryStream();

				if (i > 0) {
					importer.PaletteMode = PaletteMode.Palette16_16;
					importer.Format = ColorFormat.Indexed_4bpp;
				}

				importer.ImportBackground(emguImgs[i], nscrStr, ncgrStr, nclrStr);
				nclrStr.Position = ncgrStr.Position = nscrStr.Position = 0;

				// Only first pack file has palette file
				if (i == 0)
					packs[i] = Npck.FromBackgroundStreams(nscrStr, ncgrStr, nclrStr);
				else
					packs[i] = Npck.FromBackgroundStreams(nscrStr, ncgrStr, null);
			}

			// Dispose images
			foreach (Bitmap emgu in emguImgs)
				emgu.Dispose();

			return packs;
		}

		public static Npck FromSpriteImage(string[] images, int[] frames, Npck original)
		{
			Bitmap[] emguImages = new Bitmap[images.Length];
			for (int i = 0; i < images.Length; i++) {
				try { 
					emguImages[i] = new Bitmap(images[i]);
				} catch (Exception ex) {
					throw new FormatException("Unknown exception with: " + images[i], ex);
				}
			}

			Npck npck = FromSpriteImage(emguImages, frames, original);

			foreach (Bitmap img in emguImages)
				img.Dispose();

			return npck;
		}

		public static Npck FromSpriteImage(Bitmap[] images, int[] frames, Npck original)
		{
			SpriteImporter importer = new SpriteImporter();
			MemoryStream nclrStr = new MemoryStream();
			MemoryStream ncgrLinealStr = new MemoryStream();
			MemoryStream ncgrTiledStr = new MemoryStream();
			MemoryStream ncerStr = new MemoryStream();

			// Create sprites images to import
			// those sprite that have not been exported (they didn't have text)
			if (original[0] != null) {
				Nclr nclr = new Nclr(original[0]);
				Ncgr ncgr = new Ncgr(original[1] == null ? original[2] : original[1]);
				Ncer ncer = new Ncer(original[3]);

				// Set old settings
				importer.DispCnt = ncgr.RegDispcnt;
				importer.Quantization = new ManyFixedPaletteQuantization(nclr.GetPalettes());
				importer.OriginalPalettes = nclr.GetPalettes();
				importer.Format = nclr.Format;
				if (nclr.Format == ColorFormat.Indexed_8bpp)
					importer.PaletteMode = PaletteMode.Palette256_1;
				else
					importer.PaletteMode = PaletteMode.Palette16_16;

				int idx = 0;
				for (int i = 0; i < ncer.NumFrames; i++) {
					if (frames.Contains(i))
						importer.AddFrame(images[idx++]);
					else if (ncer != null)
						importer.AddFrame(ncer.CreateBitmap(i, ncgr, nclr), ncer.GetFrame(i));
				}
			} else {
				foreach (Bitmap img in images)
					importer.AddFrame(img);
			}

			// TEMP: Check if the files were present
			if (original[0] == null)
				Console.Write("(Warning: No palette) ");
			if (original[1] == null) {
				//Console.Write("(Warning: No HImage) ");
				ncgrTiledStr = null;
			}
			if (original[2] == null) {
				//Console.Write("(Warning: No LImage) ");
				ncgrLinealStr = null;
			}
			if (original[3] == null)
				Console.Write("(Warning: No sprite) ");
			if (original[5] == null)
				Console.Write("(Warning: No animation) ");

			importer.Generate(nclrStr, ncgrLinealStr, ncgrTiledStr, ncerStr);

			nclrStr.Position = 0;
			ncerStr.Position = 0;
			if (ncgrTiledStr != null)
				ncgrTiledStr.Position = 0;
			if (ncgrLinealStr != null)
				ncgrLinealStr.Position = 0;

			return Npck.FromSpriteStreams(ncerStr, ncgrLinealStr, ncgrTiledStr, nclrStr, original[5]);
		}

		public static Npck ChangeTextureImages(string[] images, int[] frames, Npck original)
		{
			return ChangeTextureImages(images, frames, null, original);
		}

		public static Npck ChangeTextureImages(Bitmap[] images, int[] frames, Npck original)
		{
			return ChangeTextureImages(images, frames, null, original);
		}

		public static Npck ChangeTextureImages(string[] images, int[] frames, Palette palOut, Npck original)
		{
			// Creamos las imágenes
			Bitmap[] emguImages = new Bitmap[images.Length];
			for (int i = 0; i < images.Length; i++) {
				try {
					emguImages[i] = new Bitmap(images[i]);
				} catch (Exception ex) {
					throw new FormatException("Unknown exception with: " + images[i], ex);
				}
			}

			Npck npck = ChangeTextureImages(emguImages, frames, palOut, original);

			// Liberamos recursos
			foreach (Bitmap img in emguImages)
				img.Dispose();

			return npck;
		}


		public static Npck ChangeTextureImages(Bitmap[] images, int[] frames, Palette palOut, Npck original)
		{
			// Get an original texture file to get images and the texture to modify
			long start = original[0].Position;

			Btx0 oriTexture = new Btx0(original[0]);
			original[0].Position = start;

			Btx0 newTexture = new Btx0(original[0]);
			original[0].Position = start;

			// Create importer and remove all images
			TextureImporter importer = new TextureImporter(newTexture);
			importer.RemoveImages();

			// Add images from arguments or original texture if not presents.
			List<int> frameList = frames.ToList();
			for (int i = 0; i < oriTexture.NumTextures; i++) {
				int idx = frameList.IndexOf(i);

				// Set quantization to original palette or argument palette
				Color[] originalPalette = oriTexture.GetPalette(i).GetPalette(0);
				if (idx != -1 && palOut != null)
					importer.Quantization = new FixedPaletteQuantization(palOut.GetPalette(idx));
				else
					importer.Quantization = new FixedPaletteQuantization(originalPalette);

				// Keep original color format and name
				string name = oriTexture.GetTextureName(i);
				importer.Format = oriTexture.GetImage(i).Format;

				// Keep original unknown values
				int[] texUnks = oriTexture.GetTextureUnknowns(i);
				int[] palUnks = oriTexture.GetPaletteUnknowns(i);

				// Import :D
				if (idx != -1)
					importer.AddImage(images[idx], name, texUnks, palUnks, originalPalette);
				else
					importer.AddImage(oriTexture.CreateBitmap(i), name, texUnks, palUnks);
			}

			// Write the new texture file
			MemoryStream textureStream = new MemoryStream();
			newTexture.Write(textureStream);
			textureStream.Position = 0;

			// Create a copy of the NPCK from the original
			Npck npck = new Npck();
			npck.AddSubfile(textureStream);
			for (int i = 1; i < 6; i++)
				npck.AddSubfile(original[i]);

			return npck;
		}
	}
}

