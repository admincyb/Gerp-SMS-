using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Reporting.WebForms;

namespace GTIService.Dashboard
{
    #region Dynamic RDLC Report
    /// <summary>
    /// Dynamic RDLC
    /// </summary>
    public class DynamicRdlc
    {
        #region Public Properties
        /// <summary>
        /// ReportViewer for Dynamic RDLC
        /// </summary>
        public ReportViewer Report { get; set; }
        /// <summary>
        /// DataSource for RDLC
        /// </summary>
        public DataTable ReportDataSource { get; set; }
        /// <summary>
        /// Number of Groups
        /// </summary>
        public int GroupCount { get; set; }
        /// <summary>
        /// List of Fields
        /// </summary>
        public Fields FieldsList { get; set; }
        public string LogoImage { get; set; }
        public string ReportCurrency { get; set; }
        public string Period { get; set; }
        #endregion
        #region Private Properties
        #region RDLC Parts
        /// <summary>
        /// Rdlc Start Xml
        /// </summary>
        private string RdlcStartXml
        {
            get
            {
                return @"<?xml version=""1.0"" encoding=""utf-8""?>
<Report xmlns:rd=""http://schemas.microsoft.com/SQLServer/reporting/reportdesigner"" xmlns=""http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition"">
  <DataSources>
    <DataSource Name=""ReportDataSet"">
      <ConnectionProperties>
        <DataProvider>System.Data.DataSet</DataProvider>
        <ConnectString>/* Local Connection */</ConnectString>
      </ConnectionProperties>
      <rd:DataSourceID>56c1b68a-3ff7-4a90-b92b-d48fee588acd</rd:DataSourceID>
    </DataSource>
  </DataSources>
  <DataSets>
    <DataSet Name=""DataSet1"">
      <Fields>
        <Field Name=""Field1"">
          <DataField>Field1</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""Field2"">
          <DataField>Field2</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""Field3"">
          <DataField>Field3</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""Field4"">
          <DataField>Field4</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""Field5"">
          <DataField>Field5</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""Field6"">
          <DataField>Field6</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""Field7"">
          <DataField>Field7</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""Field8"">
          <DataField>Field8</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""Field9"">
          <DataField>Field9</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""Field10"">
          <DataField>Field10</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""Field11"">
          <DataField>Field11</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""Field12"">
          <DataField>Field12</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""Field13"">
          <DataField>Field13</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""Field14"">
          <DataField>Field14</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""Field15"">
          <DataField>Field15</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""Field16"">
          <DataField>Field16</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
        <Field Name=""Cost"">
          <DataField>Cost</DataField>
          <rd:TypeName>System.Decimal</rd:TypeName>
        </Field>
      </Fields>
      <Query>
        <DataSourceName>ReportDataSet</DataSourceName>
        <CommandText>/* Local Query */</CommandText>
      </Query>
      <rd:DataSetInfo>
        <rd:DataSetName>ReportDataSet</rd:DataSetName>
        <rd:SchemaPath />
        <rd:TableName>CostDetails</rd:TableName>
        <rd:TableAdapterFillMethod />
        <rd:TableAdapterGetDataMethod />
        <rd:TableAdapterName />
      </rd:DataSetInfo>
    </DataSet>
  </DataSets>
  <Body>
    <ReportItems>
      <Tablix Name=""Tablix1"">
        <TablixBody>";
            }
        }
        /// <summary>
        /// Rdlc Header Row Xml
        /// </summary>
        private string RdlcHeaderRowXml
        {
            get
            {
                return @"<TablixRows>
            <TablixRow>
              <Height>0.25in</Height>
              <TablixCells>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox1"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Parameters!Title1.Value</Value>
                              <Style>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox1</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox3"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Parameters!Title2.Value</Value>
                              <Style>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox3</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox5"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Parameters!Title3.Value</Value>
                              <Style>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox5</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox27"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Parameters!Title4.Value</Value>
                              <Style>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox27</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox2"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Parameters!Title5.Value</Value>
                              <Style>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox2</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox10"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Parameters!Title6.Value</Value>
                              <Style>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox10</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox21"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Parameters!Title7.Value</Value>
                              <Style>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox21</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox33"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Parameters!Title8.Value</Value>
                              <Style>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox33</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox38"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Parameters!Title9.Value</Value>
                              <Style>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox38</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox43"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Parameters!Title10.Value</Value>
                              <Style>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox43</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox20"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Parameters!Title11.Value</Value>
                              <Style>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox20</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox50"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Parameters!Title12.Value</Value>
                              <Style>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox50</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox57"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Parameters!Title13.Value</Value>
                              <Style>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox57</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox64"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Parameters!Title14.Value</Value>
                              <Style>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox64</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox71"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Parameters!Title15.Value</Value>
                              <Style>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox71</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox81"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Parameters!Title16.Value</Value>
                              <Style>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox81</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox7"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=""Cost("" + Parameters!ReportCurrency.Value + "")""</Value>
                              <Style>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox7</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
              </TablixCells>
            </TablixRow>";
            }
        }
        /// <summary>
        /// Rdlc Group1 Row Xml
        /// </summary>
        private string RdlcGroup1Xml
        {
            get
            {
                return @"  <TablixRow>
              <Height>0.2in</Height>
              <TablixCells>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Field1"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Code.HTMLDecode(Fields!Field1.Value)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Field1</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox12"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox12</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox13"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox13</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox28"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox28</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox4"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox4</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox11"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox11</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox22"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox22</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox34"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox34</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox39"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox39</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox44"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox44</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox32"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox32</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox51"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox51</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox58"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox58</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox65"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox65</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox72"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox72</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox88"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox88</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox14"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=FormatNumber(Round(CDbl(Sum(Fields!Cost.Value, ""Group1"")),2),2,TriState.True)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style>
                            <TextAlign>Right</TextAlign>
                          </Style>
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox14</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
              </TablixCells>
            </TablixRow>";
            }
        }
        /// <summary>
        /// Rdlc Group2 Row Xml
        /// </summary>
        private string RdlcGroup2Xml
        {
            get
            {
                return @" <TablixRow>
              <Height>0.2in</Height>
              <TablixCells>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox15"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox15</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Field2"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Code.HTMLDecode(Fields!Field2.Value)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Field2</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox17"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Format>Verdana, 7pt, Normal, Normal, Default</Format>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox17</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox29"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox29</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox6"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox6</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox16"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox16</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox25"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox25</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox35"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox35</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox40"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox40</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox45"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox45</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox37"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox37</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox52"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox52</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox59"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox59</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox66"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox66</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox73"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox73</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox89"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox89</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox18"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=FormatNumber(Round(CDbl(Sum(Fields!Cost.Value, ""Group2"")),2),2,TriState.True)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style>
                            <TextAlign>Right</TextAlign>
                          </Style>
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox18</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
              </TablixCells>
            </TablixRow>";
            }
        }
        /// <summary>
        /// Rdlc Group3 Row Xml
        /// </summary>
        private string RdlcGroup3Xml
        {
            get
            {
                return @" <TablixRow>
              <Height>0.2in</Height>
              <TablixCells>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox23"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox23</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox24"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox24</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Field3"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Code.HTMLDecode(Fields!Field3.Value)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Field3</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox30"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox30</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox8"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox8</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox19"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox19</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox31"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox31</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox36"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox36</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox41"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox41</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox46"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox46</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox42"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox42</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox53"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox53</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox60"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox60</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox67"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox67</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox74"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox74</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox91"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox91</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox26"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=FormatNumber(Round(CDbl(Sum(Fields!Cost.Value, ""Group3"")),2),2,TriState.True)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style>
                            <TextAlign>Right</TextAlign>
                          </Style>
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox26</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
              </TablixCells>
            </TablixRow>";
            }
        }
        /// <summary>
        /// Rdlc Group4 Row Xml
        /// </summary>
        private string RdlcGroup4Xml
        {
            get
            {
                return @" <TablixRow>
              <Height>0.2in</Height>
              <TablixCells>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox78"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox78</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox79"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox79</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox80"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox80</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Field41"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Code.HTMLDecode(Fields!Field4.Value)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Field41</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox82"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox82</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox83"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox83</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox84"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox84</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox85"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox85</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox86"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox86</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox87"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox87</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox47"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox47</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox54"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox54</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox61"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox61</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox68"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox68</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox75"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox75</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox92"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox92</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox90"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=FormatNumber(Round(CDbl(Sum(Fields!Cost.Value, ""Group4"")),2),2,TriState.True)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style>
                            <TextAlign>Right</TextAlign>
                          </Style>
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox90</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
              </TablixCells>
            </TablixRow>";
            }
        }
        /// <summary>
        /// Rdlc Group5 Row Xml
        /// </summary>
        private string RdlcGroup5Xml
        {
            get
            {
                return @" <TablixRow>
              <Height>0.2in</Height>
              <TablixCells>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox97"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox97</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox98"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox98</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox99"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox99</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox100"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox100</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Field51"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Code.HTMLDecode(Fields!Field5.Value)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Field51</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox102"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox102</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox103"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox103</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox104"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox104</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox105"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox105</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox106"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox106</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox48"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox48</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox55"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox55</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox62"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox62</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox69"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox69</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox76"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox76</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox93"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value />
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox93</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox109"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=FormatNumber(Round(CDbl(Sum(Fields!Cost.Value, ""Group5"")),2),2,TriState.True)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style>
                            <TextAlign>Right</TextAlign>
                          </Style>
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox109</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
              </TablixCells>
            </TablixRow>";
            }
        }
        /// <summary>
        /// Rdlc Details(Last) Row Xml
        /// </summary>
        private string RdlcDetailsRowXml
        {
            get
            {
                return @" <TablixRow>
              <Height>0.2in</Height>
              <TablixCells>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Field11"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=IIF(Parameters!GroupCount.Value&lt;1, Code.HTMLDecode(Fields!Field1.Value), """")</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Field11</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Field21"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=IIF(Parameters!GroupCount.Value&lt;2, Code.HTMLDecode(Fields!Field2.Value), """")</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Field21</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Field31"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=IIF(Parameters!GroupCount.Value&lt;3, Code.HTMLDecode(Fields!Field3.Value), """")</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Field31</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Field4"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=IIF(Parameters!GroupCount.Value&lt;4, Code.HTMLDecode(Fields!Field4.Value), """")</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Field4</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Field5"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=IIF(Parameters!GroupCount.Value&lt;5, Code.HTMLDecode(Fields!Field5.Value), """")</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Field5</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Field6"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Code.HTMLDecode(Fields!Field6.Value)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Field6</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Field7"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Code.HTMLDecode(Fields!Field7.Value)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Field7</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Field8"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Code.HTMLDecode(Fields!Field8.Value)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Field8</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Field9"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Code.HTMLDecode(Fields!Field9.Value)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Field9</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Field10"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Code.HTMLDecode(Fields!Field10.Value)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Field10</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox49"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Code.HTMLDecode(Fields!Field11.Value)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox49</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox56"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Code.HTMLDecode(Fields!Field12.Value)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox56</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox63"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Code.HTMLDecode(Fields!Field13.Value)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox63</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox70"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Code.HTMLDecode(Fields!Field14.Value)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox70</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox77"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Code.HTMLDecode(Fields!Field15.Value)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox77</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Textbox94"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Code.HTMLDecode(Fields!Field16.Value)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox94</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name=""Cost"">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=FormatNumber(Round(CDbl(Fields!Cost.Value), 2),TriState.True)</Value>
                              <Style>
                                <FontStyle>Normal</FontStyle>
                                <FontFamily>Verdana</FontFamily>
                                <FontSize>7pt</FontSize>
                                <FontWeight>Normal</FontWeight>
                                <Color>#151515</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style>
                            <TextAlign>Right</TextAlign>
                          </Style>
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Cost</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>#c6d3e7</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
              </TablixCells>
            </TablixRow>
          </TablixRows>
        </TablixBody>";
            }
        }
        /// <summary>
        /// Rdlc Column Hierarchy Xml
        /// </summary>
        private string RdlcColumnHierarchyXml
        {
            get
            {
                return @" <TablixColumnHierarchy>
          <TablixMembers>
            <TablixMember>
              <Visibility>
                <Hidden>=IIF(Parameters!ColumnCount.Value&gt;=1, False, True)</Hidden>
              </Visibility>
            </TablixMember>
            <TablixMember>
              <Visibility>
                <Hidden>=IIF(Parameters!ColumnCount.Value&gt;=2, False, True)</Hidden>
              </Visibility>
            </TablixMember>
            <TablixMember>
              <Visibility>
                <Hidden>=IIF(Parameters!ColumnCount.Value&gt;=3, False, True)</Hidden>
              </Visibility>
            </TablixMember>
            <TablixMember>
              <Visibility>
                <Hidden>=IIF(Parameters!ColumnCount.Value&gt;=4, False, True)</Hidden>
              </Visibility>
            </TablixMember>
            <TablixMember>
              <Visibility>
                <Hidden>=IIF(Parameters!ColumnCount.Value&gt;=5, False, True)</Hidden>
              </Visibility>
            </TablixMember>
            <TablixMember>
              <Visibility>
                <Hidden>=IIF(Parameters!ColumnCount.Value&gt;=6, False, True)</Hidden>
              </Visibility>
            </TablixMember>
            <TablixMember>
              <Visibility>
                <Hidden>=IIF(Parameters!ColumnCount.Value&gt;=7, False, True)</Hidden>
              </Visibility>
            </TablixMember>
            <TablixMember>
              <Visibility>
                <Hidden>=IIF(Parameters!ColumnCount.Value&gt;=8, False, True)</Hidden>
              </Visibility>
            </TablixMember>
            <TablixMember>
              <Visibility>
                <Hidden>=IIF(Parameters!ColumnCount.Value&gt;=9, False, True)</Hidden>
              </Visibility>
            </TablixMember>
            <TablixMember>
              <Visibility>
                <Hidden>=IIF(Parameters!ColumnCount.Value&gt;=10, False, True)</Hidden>
              </Visibility>
            </TablixMember>
            <TablixMember>
              <Visibility>
                <Hidden>=IIF(Parameters!ColumnCount.Value&gt;=11, False, True)</Hidden>
              </Visibility>
            </TablixMember>
            <TablixMember>
              <Visibility>
                <Hidden>=IIF(Parameters!ColumnCount.Value&gt;=12, False, True)</Hidden>
              </Visibility>
            </TablixMember>
            <TablixMember>
              <Visibility>
                <Hidden>=IIF(Parameters!ColumnCount.Value&gt;=13, False, True)</Hidden>
              </Visibility>
            </TablixMember>
            <TablixMember>
              <Visibility>
                <Hidden>=IIF(Parameters!ColumnCount.Value&gt;=14, False, True)</Hidden>
              </Visibility>
            </TablixMember>
            <TablixMember>
              <Visibility>
                <Hidden>=IIF(Parameters!ColumnCount.Value&gt;=15, False, True)</Hidden>
              </Visibility>
            </TablixMember>
            <TablixMember>
              <Visibility>
                <Hidden>=IIF(Parameters!ColumnCount.Value&gt;=16, False, True)</Hidden>
              </Visibility>
            </TablixMember>
            <TablixMember />
          </TablixMembers>
        </TablixColumnHierarchy>";
            }
        }
        /// <summary>
        /// Rdlc End Xml
        /// </summary>
        private string RdlcEndXml
        {
            get
            {
                return @"  <DataSetName>DataSet1</DataSetName>
        <Left>0.0625in</Left>
        <Height>1.45in</Height>
        <Width>8.29994in</Width>
        <Style>
          <Border>
            <Style>None</Style>
          </Border>
        </Style>
      </Tablix>
    </ReportItems>
    <Height>1.47083in</Height>
    <Style />
  </Body>
  <ReportParameters>
    <ReportParameter Name=""ColumnCount"">
      <DataType>Integer</DataType>
      <Prompt>ReportParameter1</Prompt>
    </ReportParameter>
    <ReportParameter Name=""GroupCount"">
      <DataType>Integer</DataType>
      <Prompt>ReportParameter2</Prompt>
    </ReportParameter>
    <ReportParameter Name=""Period"">
      <DataType>String</DataType>
      <Prompt>Period</Prompt>
    </ReportParameter>
    <ReportParameter Name=""Title1"">
      <DataType>String</DataType>
      <Nullable>true</Nullable>
      <AllowBlank>true</AllowBlank>
      <Prompt>ReportParameter3</Prompt>
    </ReportParameter>
    <ReportParameter Name=""Title2"">
      <DataType>String</DataType>
      <Nullable>true</Nullable>
      <AllowBlank>true</AllowBlank>
      <Prompt>ReportParameter4</Prompt>
    </ReportParameter>
    <ReportParameter Name=""Title3"">
      <DataType>String</DataType>
      <Nullable>true</Nullable>
      <AllowBlank>true</AllowBlank>
      <Prompt>ReportParameter5</Prompt>
    </ReportParameter>
    <ReportParameter Name=""Title4"">
      <DataType>String</DataType>
      <Nullable>true</Nullable>
      <AllowBlank>true</AllowBlank>
      <Prompt>ReportParameter6</Prompt>
    </ReportParameter>
    <ReportParameter Name=""Title5"">
      <DataType>String</DataType>
      <Nullable>true</Nullable>
      <AllowBlank>true</AllowBlank>
      <Prompt>ReportParameter7</Prompt>
    </ReportParameter>
    <ReportParameter Name=""Title6"">
      <DataType>String</DataType>
      <Nullable>true</Nullable>
      <AllowBlank>true</AllowBlank>
      <Prompt>ReportParameter8</Prompt>
    </ReportParameter>
    <ReportParameter Name=""Title7"">
      <DataType>String</DataType>
      <Nullable>true</Nullable>
      <AllowBlank>true</AllowBlank>
      <Prompt>ReportParameter9</Prompt>
    </ReportParameter>
    <ReportParameter Name=""Title8"">
      <DataType>String</DataType>
      <Nullable>true</Nullable>
      <AllowBlank>true</AllowBlank>
      <Prompt>ReportParameter10</Prompt>
    </ReportParameter>
    <ReportParameter Name=""Title9"">
      <DataType>String</DataType>
      <Nullable>true</Nullable>
      <AllowBlank>true</AllowBlank>
      <Prompt>ReportParameter11</Prompt>
    </ReportParameter>
    <ReportParameter Name=""Title10"">
      <DataType>String</DataType>
      <Nullable>true</Nullable>
      <AllowBlank>true</AllowBlank>
      <Prompt>ReportParameter12</Prompt>
    </ReportParameter>
    <ReportParameter Name=""Title11"">
      <DataType>String</DataType>
      <Nullable>true</Nullable>
      <AllowBlank>true</AllowBlank>
      <Prompt>ReportParameter13</Prompt>
    </ReportParameter>
    <ReportParameter Name=""Title12"">
      <DataType>String</DataType>
      <Nullable>true</Nullable>
      <AllowBlank>true</AllowBlank>
      <Prompt>ReportParameter14</Prompt>
    </ReportParameter>
    <ReportParameter Name=""Title13"">
      <DataType>String</DataType>
      <Nullable>true</Nullable>
      <AllowBlank>true</AllowBlank>
      <Prompt>ReportParameter15</Prompt>
    </ReportParameter>
    <ReportParameter Name=""Title14"">
      <DataType>String</DataType>
      <Nullable>true</Nullable>
      <AllowBlank>true</AllowBlank>
      <Prompt>ReportParameter16</Prompt>
    </ReportParameter>
    <ReportParameter Name=""Title15"">
      <DataType>String</DataType>
      <Nullable>true</Nullable>
      <AllowBlank>true</AllowBlank>
      <Prompt>ReportParameter17</Prompt>
    </ReportParameter>
    <ReportParameter Name=""Title16"">
      <DataType>String</DataType>
      <Nullable>true</Nullable>
      <AllowBlank>true</AllowBlank>
      <Prompt>ReportParameter18</Prompt>
    </ReportParameter>
    <ReportParameter Name=""HeaderImage"">
      <DataType>String</DataType>
      <Nullable>true</Nullable>
      <AllowBlank>true</AllowBlank>
      <Prompt>ReportParameter19</Prompt>
    </ReportParameter>
    <ReportParameter Name=""ReportCurrency"">
      <DataType>String</DataType>
      <Nullable>true</Nullable>
      <AllowBlank>true</AllowBlank>
      <Prompt>ReportCurrency</Prompt>
    </ReportParameter>
  </ReportParameters>
  <Code>
        Function HTMLDecode(byVal EncodedString) As String
        Dim DecodedString, i
        DecodedString = EncodedString
        DecodedString = Replace(DecodedString, ""&amp;quot;"", chr(34) )
        DecodedString = Replace(DecodedString, ""&amp;lt;""  , chr(60) )
        DecodedString = Replace(DecodedString, ""&amp;gt;""  , chr(62) )
        DecodedString = Replace(DecodedString, ""&amp;amp;"" , chr(38) )
        DecodedString = Replace(DecodedString, ""&amp;nbsp;"", chr(32) )
        For i = 1 to 255
        DecodedString = Replace(DecodedString, ""&amp;#"" &amp; i &amp; "";"", chr( i ) )
        Next
        Return DecodedString
        End Function
    </Code>
  <Width>8.4in</Width>
  <Page>
    <PageHeader>
      <Height>0.20624in</Height>
      <PrintOnFirstPage>true</PrintOnFirstPage>
      <PrintOnLastPage>true</PrintOnLastPage>
      <ReportItems>
        <Textbox Name=""Textbox101"">
          <CanGrow>true</CanGrow>
          <KeepTogether>true</KeepTogether>
          <Paragraphs>
            <Paragraph>
              <TextRuns>
                <TextRun>
                  <Value>=Parameters!Period.Value</Value>
                  <Style>
                    <FontFamily>Verdana</FontFamily>
                    <FontSize>7pt</FontSize>
                    <FontWeight>Bold</FontWeight>
                    <Color>#151515</Color>
                  </Style>
                </TextRun>
              </TextRuns>
              <Style>
                <TextAlign>Left</TextAlign>
              </Style>
            </Paragraph>
          </Paragraphs>
          <rd:DefaultName>Textbox3</rd:DefaultName>
          <Top>0.00347in</Top>
          <Left>3.24451in</Left>
          <Height>0.1993in</Height>
          <Width>1.75348in</Width>
          <Style>
            <Border>
              <Style>None</Style>
            </Border>
            <VerticalAlign>Top</VerticalAlign>
          </Style>
        </Textbox>
        <Textbox Name=""Textbox107"">
          <CanGrow>true</CanGrow>
          <KeepTogether>true</KeepTogether>
          <Paragraphs>
            <Paragraph>
              <TextRuns>
                <TextRun>
                  <Value>Report Name : </Value>
                  <Style>
                    <FontFamily>Verdana</FontFamily>
                    <FontSize>7pt</FontSize>
                    <FontWeight>Normal</FontWeight>
                    <Color>#151515</Color>
                  </Style>
                </TextRun>
                <TextRun>
                  <Value>Cost Analysis Report</Value>
                  <Style>
                    <FontFamily>Verdana</FontFamily>
                    <FontSize>7pt</FontSize>
                    <FontWeight>Bold</FontWeight>
                    <Color>#151515</Color>
                  </Style>
                </TextRun>
                <TextRun>
                  <Value> for theperiod</Value>
                  <Style>
                    <FontFamily>Verdana</FontFamily>
                    <FontSize>7pt</FontSize>
                    <FontWeight>Normal</FontWeight>
                    <Color>#151515</Color>
                  </Style>
                </TextRun>
              </TextRuns>
              <Style>
                <TextAlign>Left</TextAlign>
              </Style>
            </Paragraph>
          </Paragraphs>
          <rd:DefaultName>Textbox3</rd:DefaultName>
          <Top>0.00347in</Top>
          <Left>0.02701in</Left>
          <Height>0.1993in</Height>
          <Width>3.18334in</Width>
          <ZIndex>1</ZIndex>
          <Style>
            <Border>
              <Style>None</Style>
            </Border>
            <VerticalAlign>Top</VerticalAlign>
          </Style>
        </Textbox>
      </ReportItems>
      <Style>
        <Border>
          <Style>None</Style>
        </Border>
      </Style>
    </PageHeader>
    <PageFooter>
      <Height>0.19444in</Height>
      <PrintOnFirstPage>true</PrintOnFirstPage>
      <PrintOnLastPage>true</PrintOnLastPage>
      <ReportItems>
        <Textbox Name=""Textbox95"">
          <CanGrow>true</CanGrow>
          <KeepTogether>true</KeepTogether>
          <Paragraphs>
            <Paragraph>
              <TextRuns>
                <TextRun>
                  <Value>Spice Jet</Value>
                  <Style>
                    <FontSize>7pt</FontSize>
                    <FontWeight>Bold</FontWeight>
                    <Color>#515151</Color>
                  </Style>
                </TextRun>
              </TextRuns>
              <Style />
            </Paragraph>
          </Paragraphs>
          <rd:DefaultName>Textbox95</rd:DefaultName>
          <Top>0.00694in</Top>
          <Left>0.075in</Left>
          <Height>0.1875in</Height>
          <Width>0.48958in</Width>
          <Style>
            <Border>
              <Style>None</Style>
            </Border>
            <PaddingLeft>2pt</PaddingLeft>
            <PaddingRight>2pt</PaddingRight>
            <PaddingTop>2pt</PaddingTop>
            <PaddingBottom>2pt</PaddingBottom>
          </Style>
        </Textbox>
        <Textbox Name=""Textbox96"">
          <CanGrow>true</CanGrow>
          <KeepTogether>true</KeepTogether>
          <Paragraphs>
            <Paragraph>
              <TextRuns>
                <TextRun>
                  <Value>Generated from gAero Suite</Value>
                  <Style>
                    <FontSize>7pt</FontSize>
                    <FontWeight>Bold</FontWeight>
                    <Color>#515151</Color>
                  </Style>
                </TextRun>
              </TextRuns>
              <Style />
            </Paragraph>
          </Paragraphs>
          <rd:DefaultName>Textbox95</rd:DefaultName>
          <Left>6.70956in</Left>
          <Height>0.1875in</Height>
          <Width>1.36128in</Width>
          <ZIndex>1</ZIndex>
          <Style>
            <Border>
              <Style>None</Style>
            </Border>
            <PaddingLeft>2pt</PaddingLeft>
            <PaddingRight>2pt</PaddingRight>
            <PaddingTop>2pt</PaddingTop>
            <PaddingBottom>2pt</PaddingBottom>
          </Style>
        </Textbox>
      </ReportItems>
      <Style>
        <Border>
          <Style>None</Style>
        </Border>
      </Style>
    </PageFooter>
    <PageHeight>8.5in</PageHeight>
    <PageWidth>11in</PageWidth>
    <LeftMargin>0.2in</LeftMargin>
    <RightMargin>0.2in</RightMargin>
    <TopMargin>0.2in</TopMargin>
    <BottomMargin>0.2in</BottomMargin>
    <Style />
  </Page>
  <rd:ReportID>234d0b82-c364-4932-b946-65eae224d313</rd:ReportID>
  <rd:ReportUnitType>Inch</rd:ReportUnitType>
</Report>";
            }
        }
        #endregion
        private int MaxColumns { get { return 17; } }
        private string DefaultWidth { get { return "0.03125in"; } }
        private string CostWidth { get { return "0.9in"; } }
        #endregion
        #region Public Methods

        /// <summary>
        /// Bind Dynamic RDLC
        /// </summary>
        public void BindRdlc()
        {
            DataTable dtCost;
            LocalReport costLocalRpt;
            ReportDataSource costDataSource;
            string rowHeirarchy;
            string tablixColumns;
            int index = 1;

            try
            {
                if (this.Report != null && this.ReportDataSource != null && this.FieldsList != null)
                {
                    this.Report.Visible = true;
                    this.Report.LocalReport.DataSources.Clear();
                    this.Report.LocalReport.EnableExternalImages = true;
                    costLocalRpt = this.Report.LocalReport;

                    //Get Formatted Data Source
                    dtCost = CommonFunctions.GetDrlcFormattedTable(this.ReportDataSource);
                    costDataSource = new ReportDataSource("DataSet1", dtCost);

                    #region Dynamic RDLC
                    //Dynamic Column Size
                    tablixColumns = GetTablixColumns();
                    //Dynamic Row Hierarchy
                    rowHeirarchy = GetHierarchyXml();
                    //Dynamic RDLC Definition
                    using (TextReader reader = new StringReader(
                        //Rdlc Start Portion + Headings
                        RdlcStartXml
                        //Rdlc Dynamic Columns
                        + tablixColumns
                        //Rdlc Header Row
                        + RdlcHeaderRowXml
                        //Rdlc Group1 Row
                        + (this.GroupCount >= 1 ? RdlcGroup1Xml : string.Empty)
                        //Rdlc Group2 Row
                        + (this.GroupCount >= 2 ? RdlcGroup2Xml : string.Empty)
                        //Rdlc Group3 Row
                        + (this.GroupCount >= 3 ? RdlcGroup3Xml : string.Empty)
                        //Rdlc Group4 Row
                        + (this.GroupCount >= 4 ? RdlcGroup4Xml : string.Empty)
                        //Rdlc Group5 Row
                        + (this.GroupCount >= 5 ? RdlcGroup5Xml : string.Empty)
                        //Rdlc Details(Last) Row
                        + RdlcDetailsRowXml
                        //Rdlc Column Hierarchy
                        + RdlcColumnHierarchyXml
                        //Genarated Row Hierarchy String
                        + rowHeirarchy
                        //Rdlc End Portion
                        + RdlcEndXml
                        ))
                    {
                        costLocalRpt.LoadReportDefinition(reader);
                    }
                    #endregion

                    //To Test Static RDLC File
                    //costLocalRpt.ReportPath = "D:\\TFS Projects\\gAeroSuite\\gAero Suite\\gAeroSuite\\gAeroSuite\\gAeroSuite\\gComs\\Reports\\CostAnalysisTestReport.rdlc";

                    costLocalRpt.DataSources.Add(costDataSource);

                    //Column Visibility Parameter
                    ReportParameter columnCount = new ReportParameter();
                    columnCount.Name = "ColumnCount";
                    columnCount.Values.Add(this.FieldsList.Field.Count.ToString());
                    costLocalRpt.SetParameters(columnCount);

                    //Group Count Parameter
                    ReportParameter grpCount = new ReportParameter();
                    grpCount.Name = "GroupCount";
                    grpCount.Values.Add(this.GroupCount.ToString());
                    costLocalRpt.SetParameters(grpCount);

                    //Set Title Parameters
                    foreach (Field fld in this.FieldsList.Field)
                    {
                        ReportParameter fieldHeading = new ReportParameter();
                        fieldHeading.Name = "Title" + index.ToString();
                        fieldHeading.Values.Add(fld.Title);
                        costLocalRpt.SetParameters(fieldHeading);
                        index++;
                    }
                    //Set Report Currency
                    //Set Logo Image
                    if (!string.IsNullOrEmpty(this.ReportCurrency))
                    {
                        ReportParameter paramReportCurr = new ReportParameter();
                        paramReportCurr.Name = "ReportCurrency";
                        paramReportCurr.Values.Add(ReportCurrency);
                        costLocalRpt.SetParameters(paramReportCurr);
                    }
                    //Set Logo Image
                    if (!string.IsNullOrEmpty(this.LogoImage))
                    {
                        ReportParameter paramLogo = new ReportParameter();
                        paramLogo.Name = "HeaderImage";
                        paramLogo.Values.Add(LogoImage);
                        costLocalRpt.SetParameters(paramLogo);
                    }

                    //Set Period
                    if (!string.IsNullOrEmpty(this.Period))
                    {
                        ReportParameter paramPeriod = new ReportParameter();
                        paramPeriod.Name = "Period";
                        paramPeriod.Values.Add(Period);
                        costLocalRpt.SetParameters(paramPeriod);
                    }

                    this.Report.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    costLocalRpt.Refresh();
                }
            }
            catch
            {

            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Genarates Row Hierarchy XML
        /// </summary>
        /// <returns></returns>
        private string GetHierarchyXml()
        {
            GroupRowHeirarchy groupRowHeirarchy;
            XmlDocument hierarchyBuilder;
            XmlNode tablixRowHierarchy;
            XmlNode members;
            XmlNode member;
            XmlNode childNode;
            string xml;
            xml = string.Empty;
            childNode = null;

            hierarchyBuilder = new XmlDocument();
            tablixRowHierarchy = hierarchyBuilder.CreateNode(XmlNodeType.Element, "TablixRowHierarchy", "");
            members = hierarchyBuilder.CreateNode(XmlNodeType.Element, "TablixMembers", "");
            member = hierarchyBuilder.CreateNode(XmlNodeType.Element, "TablixMember", "");
            members.AppendChild(member);

            //For Detaills Group
            groupRowHeirarchy = new GroupRowHeirarchy();
            groupRowHeirarchy.GroupName = "Details";
            switch (this.GroupCount)
            {
                case 1:
                    groupRowHeirarchy.ToggleItem = "Field1";
                    break;
                case 2:
                    groupRowHeirarchy.ToggleItem = "Field2";
                    break;
                case 3:
                    groupRowHeirarchy.ToggleItem = "Field3";
                    break;
                case 4:
                    groupRowHeirarchy.ToggleItem = "Field41";
                    break;
                case 5:
                    groupRowHeirarchy.ToggleItem = "Field51";
                    break;
            }
            childNode = groupRowHeirarchy.GetGroupRowHierarchy(hierarchyBuilder);

            //For All Other Groups
            for (int i = 5; i >= 1; i--)
            {
                if (i <= this.GroupCount)
                {
                    groupRowHeirarchy = new GroupRowHeirarchy();
                    groupRowHeirarchy.GroupName = "Group" + i.ToString();
                    groupRowHeirarchy.GroupField = "Field" + i.ToString();
                    groupRowHeirarchy.HasGroupVisibility = true;
                    switch (i - 1)
                    {
                        case 1:
                            groupRowHeirarchy.ToggleItem = "Field1";
                            break;
                        case 2:
                            groupRowHeirarchy.ToggleItem = "Field2";
                            break;
                        case 3:
                            groupRowHeirarchy.ToggleItem = "Field3";
                            break;
                        case 4:
                            groupRowHeirarchy.ToggleItem = "Field41";
                            break;
                        case 5:
                            groupRowHeirarchy.ToggleItem = "Field51";
                            break;
                    }
                    if (i > 1)
                        groupRowHeirarchy.KeepWithGroup = true;
                    else
                        groupRowHeirarchy.HasEmptyChild = true;

                    if (childNode != null)
                        groupRowHeirarchy.ChildNode = childNode;
                    childNode = groupRowHeirarchy.GetGroupRowHierarchy(hierarchyBuilder);
                }
            }

            //Bind Hierarchy and Get Xml
            if (childNode != null)
                members.AppendChild(childNode);
            tablixRowHierarchy.AppendChild(members);
            hierarchyBuilder.AppendChild(tablixRowHierarchy);
            xml = hierarchyBuilder.InnerXml;
            return xml;
        }

        /// <summary>
        /// Genarates Tablix Columns XML
        /// </summary>
        /// <returns></returns>
        private string GetTablixColumns()
        {
            TablixColumn rdlcColumn;
            XmlDocument columnBuilder;
            XmlNode columns;
            XmlNode column;
            Field fld;
            string xml;

            xml = string.Empty;
            column = null;

            columnBuilder = new XmlDocument();
            columns = columnBuilder.CreateNode(XmlNodeType.Element, "TablixColumns", "");
            //Dynamic Column Size
            for (int i = 0; i < this.MaxColumns; i++)
            {
                rdlcColumn = new TablixColumn();
                if (i == this.MaxColumns - 1)
                    rdlcColumn.Width = this.CostWidth;
                else if (this.FieldsList != null && this.FieldsList.Field.Count > i)
                {
                    fld = this.FieldsList.Field[i];
                    rdlcColumn.Width = fld.Width;
                }
                else
                    rdlcColumn.Width = this.DefaultWidth;

                column = rdlcColumn.GetTablixColumn(columnBuilder);
                if (column != null)
                    columns.AppendChild(column);
            }
            columnBuilder.AppendChild(columns);
            xml = columnBuilder.InnerXml;
            return xml;
        }
        #endregion
    }

    #region Field Parameters
    /// <summary>
    /// RDLC Fields
    /// </summary>
    public class Fields
    {
        [XmlElement]
        public List<Field> Field { get; set; }
        /// <summary>
        /// To Serialize the Field Parameters class object
        /// </summary>
        /// <returns></returns>
        public string XmlSerialize()
        {
            XmlSerializer ser;
            StringWriter sw;
            XmlTextWriter tw;
            string result;
            result = string.Empty;
            try
            {
                ser = new XmlSerializer(typeof(Fields));
                sw = new StringWriter();
                tw = new XmlTextWriter(sw);
                ser.Serialize(tw, this);
                result = sw.ToString();
                result = result.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
            }
            catch
            {

            }
            return result;
        }
        /// <summary>
        /// To Deserialize the Field Parameters class object
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static Fields XmlDeserialize(string xml)
        {
            XmlSerializer ser;
            Fields obj;
            obj = null;
            try
            {
                //deserialize to object
                ser = new XmlSerializer(typeof(Fields));
                obj = (Fields)ser.Deserialize(XmlReader.Create(new StringReader(xml)));
            }
            catch
            {

            }
            return obj;
        }

    }
    /// <summary>
    /// RDLC Field
    /// </summary>
    public class Field
    {
        /// <summary>
        /// Title of the DataSource Field
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Name of the DataSource Field
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Size of the RDLC Column
        /// </summary>
        public string Width { get; set; }
    }

    public class FieldSize
    {
        public const string Route = "1in";
        public const string Region = "1in";
        public const string Country = "1in";
        public const string Airport = "1in";
        public const string Leg = "1in";
        public const string Flight = "1in";
        public const string AircraftType = "1in";
        public const string Vendor = "1in";
        public const string Parent = "1in";
        public const string Element = "1in";
        public const string Cost = "1in";
    }
    #endregion
    #region Row Hierarchy
    /// <summary>
    /// Dynamic RDLC Row Hierarchy
    /// </summary>
    public class GroupRowHeirarchy
    {
        /// <summary>
        /// To Skip The Group
        /// </summary>
        public bool SkipGroup { get; set; }
        /// <summary>
        /// To Set Group Visibility
        /// </summary>
        public bool HasGroupVisibility { get; set; }
        /// <summary>
        /// To add Empty TablixMember
        /// </summary>
        public bool HasEmptyChild { get; set; }
        /// <summary>
        /// To set Group Name
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// To add TablixMember
        /// </summary>
        public XmlNode ChildNode { get; set; }
        /// <summary>
        /// To Set Visibility
        /// </summary>
        public string ToggleItem { get; set; }
        /// <summary>
        /// To Create GroupExpression
        /// </summary>
        public string GroupField { get; set; }
        /// <summary>
        /// To Set KeepWithGroup After
        /// </summary>
        public bool KeepWithGroup { get; set; }

        public XmlNode GetGroupRowHierarchy(XmlDocument hierarchyBuilder)
        {
            //Nodes for Group Hierarchy
            XmlNode member;
            XmlNode group;
            XmlAttribute groupName;
            XmlNode members;
            XmlNode emptyChildMember;

            //Node for Group Expression
            XmlNode groupExpressions;
            XmlNode groupExpression;

            //Node for Sort Expression
            XmlNode sortExpressions;
            XmlNode sortExpression;
            XmlNode sortValue;

            //Nodes to set Visibility
            XmlNode tablixMemberVisibility;
            XmlNode visibility;
            XmlNode hidden;
            XmlNode toggleItem;
            XmlNode keepWithGroup;

            //Nodes to set Member Visibility
            XmlNode groupVisibility;
            XmlNode groupHidden;
            XmlNode groupToggleItem;

            members = null;

            try
            {
                member = hierarchyBuilder.CreateNode(XmlNodeType.Element, "TablixMember", "");
                //If need to Skip Group
                if (SkipGroup)
                {
                    members = hierarchyBuilder.CreateNode(XmlNodeType.Element, "TablixMembers", "");
                    //If has ChildXml append to Member Node
                    if (this.ChildNode != null)
                    {
                        members.AppendChild(this.ChildNode);
                    }
                    member.AppendChild(members);
                }
                //Need the Group Hierarchy
                else
                {
                    //Set Group
                    group = hierarchyBuilder.CreateNode(XmlNodeType.Element, "Group", "");
                    groupName = hierarchyBuilder.CreateAttribute("Name");
                    groupName.Value = this.GroupName;
                    group.Attributes.Append(groupName);

                    //If has GroupExpressions
                    if (!string.IsNullOrEmpty(this.GroupField))
                    {
                        groupExpressions = hierarchyBuilder.CreateNode(XmlNodeType.Element, "GroupExpressions", "");
                        groupExpression = hierarchyBuilder.CreateNode(XmlNodeType.Element, "GroupExpression", "");
                        groupExpression.InnerText = "=Fields!" + this.GroupField + ".Value";
                        groupExpressions.AppendChild(groupExpression);
                        group.AppendChild(groupExpressions);
                    }
                    member.AppendChild(group);

                    //If has GroupField
                    if (!string.IsNullOrEmpty(this.GroupField))
                    {
                        sortExpressions = hierarchyBuilder.CreateNode(XmlNodeType.Element, "SortExpressions", "");
                        sortExpression = hierarchyBuilder.CreateNode(XmlNodeType.Element, "SortExpression", "");
                        sortValue = hierarchyBuilder.CreateNode(XmlNodeType.Element, "Value", "");
                        sortValue.InnerText = "=Fields!" + this.GroupField + ".Value";
                        sortExpression.AppendChild(sortValue);
                        sortExpressions.AppendChild(sortExpression);
                        member.AppendChild(sortExpressions);
                    }

                    //If has ToggleItem
                    if (!string.IsNullOrEmpty(this.ToggleItem))
                    {
                        visibility = hierarchyBuilder.CreateNode(XmlNodeType.Element, "Visibility", "");
                        hidden = hierarchyBuilder.CreateNode(XmlNodeType.Element, "Hidden", "");
                        toggleItem = hierarchyBuilder.CreateNode(XmlNodeType.Element, "ToggleItem", "");

                        hidden.InnerText = "true";
                        visibility.AppendChild(hidden);
                        toggleItem.InnerText = this.ToggleItem;
                        visibility.AppendChild(toggleItem);

                        if (this.GroupName == "Details")
                        {
                            member.AppendChild(visibility);
                        }
                        else
                        {
                            tablixMemberVisibility = hierarchyBuilder.CreateNode(XmlNodeType.Element, "TablixMember", "");
                            tablixMemberVisibility.AppendChild(visibility);
                            if (this.KeepWithGroup)
                            {
                                keepWithGroup = hierarchyBuilder.CreateNode(XmlNodeType.Element, "KeepWithGroup", "");
                                keepWithGroup.InnerText = "After";
                                tablixMemberVisibility.AppendChild(keepWithGroup);
                            }
                            members = hierarchyBuilder.CreateNode(XmlNodeType.Element, "TablixMembers", "");
                            if (this.HasEmptyChild)
                            {
                                emptyChildMember = hierarchyBuilder.CreateNode(XmlNodeType.Element, "TablixMember", "");
                                members.AppendChild(emptyChildMember);
                            }
                            members.AppendChild(tablixMemberVisibility);
                        }
                    }

                    //If has ChildXml
                    if (this.ChildNode != null)
                    {
                        if (members == null)
                        {
                            members = hierarchyBuilder.CreateNode(XmlNodeType.Element, "TablixMembers", "");
                            if (this.HasEmptyChild)
                            {
                                emptyChildMember = hierarchyBuilder.CreateNode(XmlNodeType.Element, "TablixMember", "");
                                members.AppendChild(emptyChildMember);
                            }
                        }
                        members.AppendChild(this.ChildNode);
                    }

                    if (members != null)
                        member.AppendChild(members);

                    //To Set Member Visibility
                    if (this.HasGroupVisibility && this.GroupName != "Details" && !string.IsNullOrEmpty(this.ToggleItem))
                    {
                        groupVisibility = hierarchyBuilder.CreateNode(XmlNodeType.Element, "Visibility", "");
                        groupHidden = hierarchyBuilder.CreateNode(XmlNodeType.Element, "Hidden", "");
                        groupToggleItem = hierarchyBuilder.CreateNode(XmlNodeType.Element, "ToggleItem", "");

                        groupHidden.InnerText = "true";
                        groupVisibility.AppendChild(groupHidden);
                        groupToggleItem.InnerText = this.ToggleItem;
                        groupVisibility.AppendChild(groupToggleItem);

                        member.AppendChild(groupVisibility);
                    }
                }
                return member;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
    public class TablixColumn
    {
        #region Dynamic RDLC Report
        /// <summary>
        /// Column Width
        /// </summary>
        public string Width { get; set; }
        #endregion

        public XmlNode GetTablixColumn(XmlDocument columnBuilder)
        {
            XmlNode column;
            XmlNode width;
            try
            {
                column = columnBuilder.CreateNode(XmlNodeType.Element, "TablixColumn", "");
                width = columnBuilder.CreateNode(XmlNodeType.Element, "Width", "");
                width.InnerText = this.Width;
                column.AppendChild(width);
                return column;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
    #endregion
    #endregion
}
