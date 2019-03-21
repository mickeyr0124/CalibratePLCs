Public Class AverageData

    Private mMultiplier As Single = 1
    Public Property Multiplier() As Single
        Get
            Return mMultiplier
        End Get
        Set(ByVal value As Single)
            mMultiplier = value
        End Set
    End Property

    Public Function CalcAveData(Average As String, NewData As String) As String
        'convert from strings to single
        Dim singAve As Single = Val(Average)
        Dim singNewData As Single = Val(NewData)

        'if multiplier is not 0, calc sliding window average
        If mMultiplier <> 0 Then
            Return Format((singNewData - singAve) * Multiplier + singAve, "###0.0")
            'if multiplier is 0, return new data
        Else
            Return Format(singNewData, "###0.0")
        End If

    End Function
End Class
