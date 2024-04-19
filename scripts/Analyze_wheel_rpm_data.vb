Sub Analyze_wheel_rpm_data()
'
' Analyze_wheel_rpm_data Macro
' Count potential cases of wheel slip in the wheel_rpm data
' To use, convert a TartanDrvie rosbag to .csv using https://github.com/AtsushiSakai/rosbag_to_csv
' Check the wheel_rpm data topic when converting.
' Save rosbagname_wheel_rpm.csv as an excel file. Run this macro.
' Optional: pick a threshold for suspicious wheel speed discrepencies (default set to 0.3 in line 83)
'

'
    Dim lastRow As Long
    
    Range("J1").Select
    ActiveCell.FormulaR1C1 = "spread"
    Range("J2").Select
    ActiveCell.FormulaR1C1 = "=MAX(RC[-4]:RC[-1])-MIN(RC[-4]:RC[-1])"
    Range("J2").Select
    Selection.AutoFill Destination:=Range("J2:J" & Range("I" & Rows.Count).End(xlUp).Row)
    Range("J2:J1217").Select
    Range("B:B,J:J").Select
    Range("J1").Activate
    ActiveSheet.Shapes.AddChart2(240, xlXYScatter).Select
    ActiveChart.SetSourceData Source:=Range("$B:$B,$J:$J")
    ActiveSheet.Shapes("Chart 1").IncrementLeft 3.75
    ActiveSheet.Shapes("Chart 1").IncrementTop -30.75
    ActiveSheet.Shapes("Chart 1").IncrementLeft -18.75
    ActiveSheet.Shapes("Chart 1").IncrementTop -93
    ActiveSheet.Shapes("Chart 1").IncrementLeft -366.75
    ActiveSheet.Shapes("Chart 1").IncrementTop 123
    
    Range("B:B,F:I").Select
    Range("F1").Activate
    ActiveSheet.Shapes.AddChart2(240, xlXYScatter).Select
    ActiveChart.SetSourceData Source:=Range("$B:$B,$F:$I")
    ActiveChart.ChartTitle.Select
    ActiveChart.ChartTitle.Text = "Wheel rpms"
    Selection.Format.TextFrame2.TextRange.Characters.Text = "Wheel rpms"
    With Selection.Format.TextFrame2.TextRange.Characters(1, 10).ParagraphFormat
        .TextDirection = msoTextDirectionLeftToRight
        .Alignment = msoAlignCenter
    End With
    With Selection.Format.TextFrame2.TextRange.Characters(1, 10).Font
        .BaselineOffset = 0
        .Bold = msoFalse
        .NameComplexScript = "+mn-cs"
        .NameFarEast = "+mn-ea"
        .Fill.Visible = msoTrue
        .Fill.ForeColor.RGB = RGB(89, 89, 89)
        .Fill.Transparency = 0
        .Fill.Solid
        .Size = 14
        .Italic = msoFalse
        .Kerning = 12
        .Name = "+mn-lt"
        .UnderlineStyle = msoNoUnderline
        .Spacing = 0
        .Strike = msoNoStrike
    End With
    Range("V29").Select
    
    Range("K1").Select
    ActiveCell.FormulaR1C1 = "mean"
    Range("L1").Select
    ActiveCell.FormulaR1C1 = "median"
    Range("M1").Select
    ActiveCell.FormulaR1C1 = "max"
    Range("N1").Select
    ActiveCell.FormulaR1C1 = "min"
    Range("O1").Select
    ActiveCell.FormulaR1C1 = "fast wheel sus"
    Range("P1").Select
    ActiveCell.FormulaR1C1 = "slow wheel sus"
    Range("Q1").Select
    ActiveCell.FormulaR1C1 = "either sus"
    Range("S1").Select
    ActiveCell.FormulaR1C1 = "speed diff"
    Range("T1").Select
    ActiveCell.FormulaR1C1 = "fast ratio"
    Range("U1").Select
    ActiveCell.FormulaR1C1 = "slow ratio"
    Range("S2").Select
    ActiveCell.FormulaR1C1 = "0.3"
    Range("T2").Select
    ActiveCell.FormulaR1C1 = "=1+RC[-1]/2"
    Range("U2").Select
    ActiveCell.FormulaR1C1 = "=1-RC[-2]/2"
    Range("T4").Select
    ActiveCell.FormulaR1C1 = "rows"
    Range("U4").Select
    ActiveCell.FormulaR1C1 = "seconds"
    Range("S5").Select
    ActiveCell.FormulaR1C1 = "log length"
    Range("S6").Select
    ActiveCell.FormulaR1C1 = "fast outlier"
    Range("S7").Select
    ActiveCell.FormulaR1C1 = "slow outlier"
    Range("S8").Select
    ActiveCell.FormulaR1C1 = "either"
    Range("K2").Select
    ActiveCell.FormulaR1C1 = "=AVERAGE(RC[-5]:RC[-2])"
    Range("L2").Select
    ActiveCell.FormulaR1C1 = "=MEDIAN(RC[-6]:RC[-3])"
    Range("M2").Select
    ActiveCell.FormulaR1C1 = "=MAX(RC[-7]:RC[-4])"
    Range("N2").Select
    ActiveCell.FormulaR1C1 = "=MIN(RC[-8]:RC[-5])"
    Range("O2").Select
    ActiveCell.FormulaR1C1 = "=IF(AND(RC[-2]>(RC12*R2C20), RC[-3]>6),1,0)"
    Range("P2").Select
    ActiveCell.FormulaR1C1 = "=IF(AND(RC[-2]<(RC[-4]*R2C21), RC[-4]>6),1,0)"
    Range("Q2").Select
    ActiveCell.FormulaR1C1 = "=IF((RC[-2]+RC[-1])>=1,1,0)"
    lastRow = Cells(Rows.Count, "F").End(xlUp).Row
    Range(Cells(2, "K"), Cells(lastRow, "Q")).FillDown
    Range("T5").Select
    ActiveCell.FormulaR1C1 = "=COUNT(C[-14])"
    Range("T6").Select
    ActiveCell.FormulaR1C1 = "=COUNTIF(C[-5],1)"
    Range("T7").Select
    ActiveCell.FormulaR1C1 = "=COUNTIF(C[-4],1)"
    Range("T8").Select
    ActiveCell.FormulaR1C1 = "=COUNTIF(C[-3],1)"
    Range("U5").Select
    ActiveCell.FormulaR1C1 = "=RC[-1]*0.02"
    Range("U5").Select
    Selection.AutoFill Destination:=Range("U5:U8")
    Range("U5:U8").Select
    Range("U13").Select
    
End Sub
