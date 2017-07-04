'***********************************************************************************************************************
'* Project: CmisObjectModelLibrary
'* Copyright (c) 2014, Brügmann Software GmbH, Papenburg, All rights reserved
'*
'* Contact: opensource<at>patorg.de
'* 
'* CmisObjectModelLibrary is a VB.NET implementation of the Content Management Interoperability Services (CMIS) standard
'*
'* This file is part of CmisObjectModelLibrary.
'* 
'* This library is free software; you can redistribute it and/or
'* modify it under the terms of the GNU Lesser General Public
'* License as published by the Free Software Foundation; either
'* version 3.0 of the License, or (at your option) any later version.
'*
'* This library is distributed in the hope that it will be useful,
'* but WITHOUT ANY WARRANTY; without even the implied warranty of
'* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
'* Lesser General Public License for more details.
'*
'* You should have received a copy of the GNU Lesser General Public
'* License along with this library (lgpl.txt).
'* If not, see <http://www.gnu.org/licenses/lgpl.txt>.
'***********************************************************************************************************************
Imports CmisObjectModel.Common
Imports sxs = System.Xml.Serialization

Namespace CmisObjectModel.RestAtom
   <sxs.XmlRoot("content", Namespace:=Constants.Namespaces.cmisra)>
   Partial Public Class cmisContentType

      Public Sub New(base64 As String, mediaType As String)
         _base64 = base64
         _mediatype = mediaType
      End Sub
      Public Sub New(content As Byte(), mediaType As String)
         _base64 = If(content Is Nothing, Nothing, System.Convert.ToBase64String(content))
         _mediatype = mediaType
      End Sub

      Public Shared Widening Operator CType(value As Messaging.cmisContentStreamType) As cmisContentType
         Return If(value Is Nothing, Nothing, New cmisContentType(value.Stream, value.MimeType))
      End Operator

      ''' <summary>
      ''' Converts Base64 to stream
      ''' </summary>
      ''' <returns></returns>
      ''' <remarks></remarks>
      Public Function ToStream() As IO.Stream
         If String.IsNullOrEmpty(_base64) Then
            Return Nothing
         Else
            Try
               Return New IO.MemoryStream(System.Convert.FromBase64String(_base64)) With {.Position = 0}
            Catch
               Return Nothing
            End Try
         End If
      End Function

   End Class
End Namespace