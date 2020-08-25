using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PerceptronAPI.Models;

namespace PerceptronAPI.Engine
{
    public class ExtractInsilicoMass
    {
        public double ExtractInsilicoLeftMass(int index, List<string> ListFragIon, string FragmentationType, string Type, int MatchedIndex, InsilicoMassIons InsilicoDetails)
        {
            double tempTheoretical_mz = 0.0;

            if (Type == "Left")
            {
                if (FragmentationType == "ECD" || FragmentationType == "ETD")
                {
                    ListFragIon.Add("C");
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassLeftIons[MatchedIndex];
                }

                else if (FragmentationType == "EDD" || FragmentationType == "NETD")
                {
                    ListFragIon.Add("A");
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassLeftIons[MatchedIndex];
                }
                else
                {
                    ListFragIon.Add("B");
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassLeftIons[MatchedIndex];
                }

            }
            else
            {
                ListFragIon.Add(Type); /////CHECKING CHECKING WHETHER BUG .... EXISTS....?????

                if (Type == "A'")
                {
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassLeftAo[MatchedIndex];
                }
                else if (Type == "B'")
                {
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassLeftBo[MatchedIndex];
                }
                else if (Type == "A*")
                {
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassLeftAstar[MatchedIndex];
                }
                else if (Type == "B*")
                {
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassLeftBstar[MatchedIndex];
                }

            }
            return tempTheoretical_mz;
        }
        public double ExtractInsilicoRightMass(int index, List<string> ListFragIon, string FragmentationType, string Type, int MatchedIndex, InsilicoMassIons InsilicoDetails)
        {
            double tempTheoretical_mz = 0.0;

            if (Type == "Right")
            {
                if (FragmentationType == "ECD" || FragmentationType == "ETD")
                {
                    ListFragIon.Add("Z");
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassRightIons[MatchedIndex];
                }
                else if (FragmentationType == "EDD" || FragmentationType == "NETD")
                {
                    ListFragIon.Add("X");
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassRightIons[MatchedIndex];
                }
                else
                {
                    ListFragIon.Add("Y");
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassRightIons[MatchedIndex];
                }
            }
            else
            {

                ListFragIon.Add(Type);

                if (Type == "Y'")
                {
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassRightYo[MatchedIndex];
                }
                else if (Type == "Z'")
                {
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassRightZo[MatchedIndex];
                }
                else if (Type == "Z''")
                {
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassRightZoo[MatchedIndex];
                }
                else if (Type == "Y*")
                {
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassRightYstar[MatchedIndex];
                }
            }
            return tempTheoretical_mz;
        }
    }
}