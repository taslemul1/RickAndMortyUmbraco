
# Rick and Morty Importer — Umbraco CMS Integration

##  Overview
This feature integrates the [Rick and Morty API](https://rickandmortyapi.com/) with Umbraco CMS, allowing editors to import characters directly into the CMS as structured content nodes.

Characters are organized under a **Character Container** node and include details such as name, species, gender, status, and number of episodes appeared in.

## Features
- Imports all characters from the Rick and Morty API (handles pagination).
- Prevents duplicate entries by using the API `id`.
- Supports updates to existing characters if already imported.
- Displays total episode appearances per character.
- Allows re-importing at any time via a custom backoffice dashboard.

## Data Stored
Each imported character includes:

| Property          | Source                        | Type    |
|------------------|-------------------------------|---------|
| Name             | `character.name`              | String  |
| Status           | `character.status`            | String  |
| Species          | `character.species`           | String  |
| Gender           | `character.gender`            | String  |
| Episode Count    | `character.episode.length`    | Integer |
| Rick and Morty ID| `character.id`                | String  |

## Content Structure in Umbraco

```
Content
└── Character Container (characterContainer)
    ├── Rick Sanchez (character)
    ├── Morty Smith (character)
    └── ...
```

## How to Run the Project

###  Prerequisites
- [.NET 8 SDK]
- Visual Studio 2022 or VS Code

### ▶️ Getting Started

1. **Clone the Project**
   ```bash
   git clone https://github.com/taslemul1/RickAndMortyUmbraco.git
   cd RickAndMortyUmbraco
   ```

3. **Build the Solution**
   ```bash
   dotnet build
   ```

4. **Run the Project**
   ```bash
   dotnet run
   ```

   Navigate to: `https://localhost:44352/umbraco`

5. **Initial Setup (on first run)**
   - Follow Umbraco's installation steps in the browser.

## 📁 Umbraco Setup Instructions

1. Go to **Settings > Document Types** and create the following:

### `characterContainer`
- Allowed as root: ✅
- Allows `character` as child nodes

### `character`
- Properties:
  - `status` (Textstring)
  - `species` (Textstring)
  - `gender` (Textstring)
  - `episodeCount` (Integer)
  - `rickAndMortyId` (Textstring)

## 📦 API Controller

This custom controller imports data from the Rick and Morty API:

- **Endpoint**: `POST /umbraco/api/rickmorty/import`
- **File**: `RickAndMortyController.cs`

Handles pagination and safely updates or creates new nodes.

## 🧠 Developer Notes

- To modify character mapping, edit the model in:
  ```
  /Models/RickAndMorty/Character.cs
  ```

- To change container name or alias, adjust logic in the controller.

## 🎯 Bonus Features (Ideas)
- Store episode names as tags or a multiline textbox
- Add thumbnail image previews
- Display import status in real-time using AngularJS in backoffice
- Enable deletion/reset of character data
