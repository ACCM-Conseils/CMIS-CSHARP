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

Namespace CmisObjectModel.Messaging.Responses
   Partial Public Class getContentChangesResponse
      Implements IEnumerable(Of Core.cmisObjectType)

#Region "IEnumerable"
      Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of Core.cmisObjectType) Implements System.Collections.Generic.IEnumerable(Of Core.cmisObjectType).GetEnumerator
         Return If(_objects, New cmisObjectListType()).GetEnumerator()
      End Function

      Protected Overridable Function IEnumerable_GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
         Return GetEnumerator()
      End Function
#End Region

   End Class
End Namespace