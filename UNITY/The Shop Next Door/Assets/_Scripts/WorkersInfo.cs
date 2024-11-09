using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkersInfo
{
    public int genre; //0 - male / 1 - female
    public string name;
    public int salary;
    public int priceToHire;

    public WorkersInfo(int genreNew, string nameNew, int salaryNew, int priceToHireNew)
    {
        genre = genreNew;
        name = nameNew;
        salary = salaryNew;
        priceToHire = priceToHireNew;
    }
}
