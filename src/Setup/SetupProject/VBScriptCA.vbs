Function RemoveExcelRegistryVBA()
	On Error Resume Next
	Const HKEY_USERS = &H80000003
	Dim OfficeVersions : OfficeVersions = Array("14", "15", "16")
	Dim oReg : Set oReg = GetObject("winmgmts:{impersonationLevel=impersonate}!\\.\root\default:StdRegProv")
	If Err.Number = 0 Then
		oReg.EnumKey HKEY_USERS, "", usersSubKeys
		For Each userSubkey In usersSubKeys
			For Each version In OfficeVersions
				Dim keyPath : keyPath = userSubkey & "\Software\Microsoft\Office\" & version & ".0\Excel\Options"
				dim arrValueNames, arrValueTypes, strValue
				If oReg.EnumValues(HKEY_USERS, keyPath, arrValueNames, arrValueTypes) = 0 Then
					If Not IsEmpty(arrValueNames) And IsArray(arrValueNames) Then
						For Each valueName In arrValueNames
							If InStr(1, valueName, "OPEN", vbBinaryCompare) = 1 Then
								oReg.GetStringValue HKEY_USERS, keyPath, valueName, strValue
								If Not IsEmpty(strValue) And InStr(1, strValue, "OMSFW2007", vbTextCompare) Then
									oReg.DeleteValue HKEY_USERS, keyPath, valueName
								End If
							End If
						Next
					End If
				End If
			Next
		Next
	End If
	Set oReg = Nothing
End Function