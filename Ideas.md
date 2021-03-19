# MafiaApp

## Spieler Page
- Model weil eindeutig, Attribut( ID!!!, verheiratet, lebend, anwesend, Rolle, Fähigkeiten
- in Datenbank gespeichert, für jedesmal anwesende auswählbar
- in Datenbank auch letzten Rollen usw.(gesamter Spielstatus für Absturz) gespeichert
- einfach neue Spieler erstellen

## Rollen
- Auslesen und dann Fähigkeiten und verbleibende Handlungsmöglichkeiten aus ... abrufen
- möglicherweise Seite um neue Rollen in Spiel zu integreieren, dazu aber speichern der Rollen in DB notwendig
- erstellen der Seite durch C# Code, weil immer ungleich viele Rollen und Spieler für unterschiedliche Spieleinstelungen
- Spieleinstellungs Button: welche Rolle mitspielen Sollen, wie viele Spieler pro Rolle 
- ansonsten default automatik Einstellung mit nur Standardrollen und festgelegter SpielerAnzahl pro Rolle


## Spiel Page
- Übersichtlich zum Aufklappen und interagieren, Tote grau färben, Opfer färben, (Rollen färben eher unübersichtlich)

## Verlauf
- um fehler usw. nachzuvollziehen
- Verlauf löscht sich nach neu öffnen? sonst in .txt speichern dann von dort auslesen und löschen knopf


## Fehler
- Rollen werden bei änderung nicht wieder auf None gesetzt
- Bei Progammstart sind Rollennamen wieder leer
