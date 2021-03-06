﻿using System;
using System.Collections.Generic ;
using System.IO ;
using System.Linq ;

namespace SwitchVsVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
            	if ( args.Length == 0 )
            	{
					Console.WriteLine(@"usage: SwitchVsVersion [folder] [x86|AnyCPU|2010|2008|3.5Framework|4.0Framework]");
            		return ;
            	}

                StringComparer comparer = StringComparer.InvariantCultureIgnoreCase ;

                string path;
                string version;

                if (args.Length == 1)
                {
                    path = Environment.CurrentDirectory;
                    version = args[0];
                }
                else
                {
                    path = args[0];
                    version = args[1];
                }

                if ( comparer.Compare( version ,"3.5Framework") == 0 )
                {
                    FrameworkSwitcher.ModifyAllProjectsUnderThisFolderTo( path, @"3.5" ) ;
                    return ;
                }

				if ( comparer.Compare( version ,"x86") == 0 )
                {
                    TargetPlatformSwitcher.ModifyAllProjectsUnderThisFolderTo( path, @"x86" ) ;
                    return ;
                }

				if ( comparer.Compare( version ,"AnyCPU") == 0 )
                {
                    TargetPlatformSwitcher.ModifyAllProjectsUnderThisFolderTo( path, @"AnyCPU" ) ;
                    return ;
                }

                if (comparer.Compare( version ,"4.0Framework")==0)
                {
                    FrameworkSwitcher.ModifyAllProjectsUnderThisFolderTo( path, @"4.0" ) ;
                    return ;
                }

                if (comparer.Compare( version ,"2008")==0)
                {
					switchProjectsAndSolutions(
						path, 
						new ProjectAndSolutionMappings().Select(mapping => mapping.InReverse()));

					return;
				}

                if (comparer.Compare( version ,"2010")==0)
                {
                	switchProjectsAndSolutions(
						path, 
						new ProjectAndSolutionMappings( ) ) ;
                    
					return;
                }

                Console.WriteLine(
                    string.Format( @"Invalid version '{0}'.  Use 3.5Framework, 4.0Framework, 2008, or 2010", version ) ) ;
            }
            catch (Exception e)
            {
                Console.WriteLine( @"Error: " + e.Message ) ;
            }
            finally
            {
                Console.WriteLine(@"Finished");
            }
        }

    	static void switchProjectsAndSolutions( string path, IEnumerable< Mapping > mappings )
    	{
			foreach (string wildcard in new[]
			                  	{
			                  		@"*.csproj",
			                  		@"*.sln",
			                  		@"*.vbproj" 
								})
			{
				foreach (string eachFilename in Disk.GetFiles(path, wildcard))
				{
					Disk.ModifyFile(eachFilename, mappings);
				}
			}

    	}
    }
}

