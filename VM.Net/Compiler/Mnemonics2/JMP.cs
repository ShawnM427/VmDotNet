﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Net.Common;

namespace VM.Net.Compiler.Mnemonics2
{
    /// <summary>
    /// Represents the JMP mnemonic, which is used to jump the instruction pointer to a given address. <br/>
    /// This mnemonic wraps around instruction code 0x0C
    /// </summary>
    public class JMP : Mneumonic
    {
        public JMP() : base("JMP", new byte[] { 0x0C, 0x4C }) { }

        public override void Interpret(SourceCrawler sourceCrawler, BinaryWriter output, bool isLabelScan)
        {
            // Eat whitespace to right of mnemonic
            sourceCrawler.EatWhitespace();

            // Peek to make sure next character is a register delimiter, otherwise we return
            if (sourceCrawler.Peek() == CompilerSettings.LabelDelimiter)
            {
                // Pass over the register delimiter
                sourceCrawler.CurrentNdx++;
                // Read the register
                uint location = sourceCrawler.ReadLabelLocation();

                
                // This instruction is 1 byte for instruction, and 1 word for location
                sourceCrawler.AssemblyLength += (uint)(1 + CompilerSettings.WORD_LENGTH);

                // If this is not a label scanning pass, write to output
                if (!isLabelScan)
                {
                    output.Write(ByteCodes[0]); // 0x0C
                    output.Write(location);
                }
            }
            else if (sourceCrawler.Peek() == CompilerSettings.LiteralDelimiter)
            {
                // Pass over the register delimiter
                sourceCrawler.CurrentNdx++;
                // Read the register
                uint location = sourceCrawler.ReadWordValue();
                
                // This instruction is 1 byte for instruction, and 1 word for location
                sourceCrawler.AssemblyLength += (uint)(1 + CompilerSettings.WORD_LENGTH);

                // If this is not a label scanning pass, write to output
                if (!isLabelScan)
                {
                    output.Write(ByteCodes[0]); // 0x0C
                    output.Write(location);
                }
            } else 
            if (sourceCrawler.Peek() == CompilerSettings.RegisterDelimiter)
            {
                // Pass over the register delimiter
                sourceCrawler.CurrentNdx++;
                // Read the register
                RegisterAddress sourceRegister = sourceCrawler.ReadRegister();

                // This instruction is 1 byte for instruction, and 1 word for location
                sourceCrawler.AssemblyLength += (uint)(1 + 1);

                // If this is not a label scanning pass, write to output
                if (!isLabelScan)
                {
                    output.Write(ByteCodes[1]); // 0x4C
                    output.Write((byte)sourceRegister);
                }
            }
        }
    }
}
