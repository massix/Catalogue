<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:xs="http://www.w3.org/2001/XMLSchema">

	<xsl:output method="html" omit-xml-declaration="yes" encoding="UTF-8" media-type="text/html" indent="yes"/>
	<xsl:param name="PageName"/>
	<xsl:param name="Generated"/>
	<xsl:param name="SortBy"/>
	<xsl:param name="imdb_it"/>
	<xsl:param name="imdb_univ"/>
	<xsl:param name="google"/>
	<xsl:param name="google_lang"/>
	<xsl:param name="catalogue_version"/>

	<xsl:template match="/">
		<html>
			<head>
				<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
				<meta name="Author" content="Catalogue Exporter &lt;gengarel@cs.unibo.it&gt;" />
				<style type="text/css">
					body {
						margin: 0;
						background-color: #ccc;
						font-family: Arial, Liberation Sans, sans-serif;
					}
	
					#FilmTable {
						color: black;
						padding: 8px;
						width: 100%;
	
					}
	
					div.Rounded {
						-moz-box-shadow: 3px 3px 3px #666;
						-webkit-box-shadow: 3px 3px 3px #666;
						box-shadow: 3px 3px 3px #666;
						-moz-border-radius: 15px;
						border-radius: 15px;
						border: 2px solid #ccc;
					}
	
					thead {
						border: 3px solid #98bf21;
						background-color: #A7C942;
						font-size: 1.1em;
						text-align: left;
						padding-top: 5px;
						padding-bottom: 4px;
						text-align: left;
						color: #ffffff;
						font-family: Arial, Verdana, Bitstream Vera Sans, sans-serif;
					}
		
					div#SummaryContainer {
						font-size: 1.1em;
						font-weight: bold;
						margin-bottom: 15px;
						padding: 15px;
					}
	
					div#TableContainer {
						padding: 15px;
					}

					.FilmEntryGray {
						background-color: #EAF2D3;
						color: black;
					}

					h1 {
						text-align: center;
						font-size: 4em;
					}

					a, a:visited {
						color: black;
					}

					a:hover {
						background-color: lightyellow;
						color: black;
					}
		
					div#Footer {
						border: 1px dotted black;
						margin-top: 40px;
						padding: 1em;
						font-family: monospace;
						text-align: center;
						width: auto;
					}
	
					div#BigContainer {
						margin-top: 0px;
						margin-bottom: 3em;
						height: auto;
						margin-left: 3em;
						margin-right: 3em;
						border-left: 2px solid black;
						border-right: 2px solid black;
						border-bottom: 2px solid black;
						padding: 1em;
						background-color: white;
						-moz-box-shadow: 3px 3px 3px 3px #666;
						-webkit-box-shadow: 3px 3px 3px 3px #666;
						box-shadow: 3px 3px 3px 3px #666;
					}
	
					table {
						border-collapse:collapse;
					}
					
					table, th, td {
						border: 1px solid #98bf21;
						padding: 3px 7px 2px 7px;
					}

				</style>
				<title>
					CatalogueExport
				</title>
			</head>
		
			<body>
				<div id="BigContainer">
					<h1><xsl:value-of select="$PageName"/></h1>
					<div id="SummaryContainer" class="Rounded">
						<h2>Summary for Catalogue</h2>
						<ul>
							<li>Films: <xsl:value-of select="count(//CatalogueEntry)"/></li>
							<li>Working films: <xsl:value-of select="count(//CatalogueEntry[@Works = 'True'])"/></li>
							<li>Non working films: <xsl:value-of select="count(//CatalogueEntry[@Works = 'False'])"/></li>
							<li>Page Generated: <xsl:value-of select="$Generated"/></li>
						</ul>
					</div>
					<div id="TableContainer" class="Rounded">
						<table id="FilmTable">
							<thead>
								<th>ID</th>
								<th>Title</th>
								<xsl:if test="$imdb_it = 'True'">
									<th>Link IMDb.it</th>
								</xsl:if>
								<xsl:if test="$imdb_univ = 'True'">
									<th>Link IMDb.com</th>
								</xsl:if>
								<xsl:if test="$google = 'True'">
									<th>Link Google (<xsl:value-of select="$google_lang"/>)</th>
								</xsl:if>
								<th>Page</th>
								<th>Works</th>
							</thead>
							<tbody>
								<xsl:for-each select="//CatalogueEntry">
									<xsl:sort select="."/>
									<tr>
										<xsl:choose>
											<xsl:when test="position() mod 2 = 1">
												<xsl:attribute name="class">FilmEntry</xsl:attribute>
											</xsl:when>
											<xsl:otherwise>
												<xsl:attribute name="class">FilmEntryGray</xsl:attribute>
											</xsl:otherwise>
										</xsl:choose>
										<td><xsl:value-of select="position()"/></td>
										<td><xsl:value-of select="."/></td>
										<xsl:if test="$imdb_it = 'True'">
											<td>
												<xsl:element name="a">
													<xsl:attribute name="target">_blank</xsl:attribute>
													<xsl:attribute name="href">
														http://www.imdb.it/find?s=tt&amp;q=<xsl:value-of select="."/>
													</xsl:attribute>
													IMDb.it
												</xsl:element>
											</td>
										</xsl:if>
										<xsl:if test="$imdb_univ = 'True'">
											<td>
												<xsl:element name="a">
													<xsl:attribute name="target">_blank</xsl:attribute>
													<xsl:attribute name="href">
														http://www.imdb.com/find?s=tt&amp;q=<xsl:value-of select="."/>
													</xsl:attribute>
													IMDb.com
												</xsl:element>
											</td>
										</xsl:if>
										<xsl:if test="$google = 'True'">
											<td>
												<xsl:element name="a">
													<xsl:attribute name="target">_blank</xsl:attribute>
													<xsl:attribute name="href">
														http://www.google.com/#hl=<xsl:value-of select="$google_lang"/>&amp;source=hp&amp;q=<xsl:value-of select="."/>
													</xsl:attribute>
													Google
												</xsl:element>
											</td>
										</xsl:if>
										<td><xsl:value-of select="@PageNo"/></td>
										<td>
											<xsl:choose>
												<xsl:when test="@Works = 'True'">
													<b>OK</b>
												</xsl:when>
												<xsl:otherwise>
													<i>Not working</i>
												</xsl:otherwise>
											</xsl:choose>
										</td>
									</tr>
								</xsl:for-each>
							</tbody>
						</table>
					</div>
					<div id="Footer">
						<a href="http://catalogue.casafamelica.info">Generated with Catalogue <xsl:value-of select="$catalogue_version"/></a>, Catalogue is released under MIT/X11 License, Copyright (C) 2011 Massimo Gengarelli &lt;<a href="mailto:gengarel@cs.unibo.it">gengarel@cs.unibo.it</a>&gt;
					</div>
				</div>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
