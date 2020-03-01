using UnityEngine;
using System.Collections;
using System.Collections.Generic; //Needed for Lists
using System.Xml; //Needed for XML functionality
using System.Xml.Serialization; //Needed for XML Functionality
using System.IO;
using System.Xml.Linq; //Needed for XDocument

public class ParseXML : MonoBehaviour
{

    public TextAsset XMLTreeFile;
    public string xmlName;

    void Start()
    {
        xmlName = XMLTreeFile.text;
    }


    public List<CNode> LoadFile(string docName)
    {
        XmlDocument XMLDoc = new XmlDocument();
        XMLDoc.Load(new StringReader(docName));




    }



}
