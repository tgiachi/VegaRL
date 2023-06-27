# Change Log

All notable changes to this project will be documented in this file. See [versionize](https://github.com/versionize/versionize) for commit guidelines.

<a name="0.0.1"></a>
## [0.0.1](https://www.github.com/tgiachi/VegaRL/releases/tag/v0.0.1) (2023-6-27)

### Features

* add interfaces for entities that have tile properties ([b3a2b03](https://www.github.com/tgiachi/VegaRL/commit/b3a2b03feee66a3f2316d0931755d95bc50adc2b))
* add new attributes to support dependency injection and text production ([387ff9c](https://www.github.com/tgiachi/VegaRL/commit/387ff9c7aadfa81adb2e1711459625cfe6c31cae))
* add new classes and interfaces for actions and action executors ([131a293](https://www.github.com/tgiachi/VegaRL/commit/131a293e32ab64195ec2b3f2ffbe5dd4a960e607))
* add new classes and interfaces for map and services ([5cb5d4e](https://www.github.com/tgiachi/VegaRL/commit/5cb5d4ef46e63c38fa2fe8a1f3ed08214ceee05f))
* add new classes and interfaces to the project ([678b8a0](https://www.github.com/tgiachi/VegaRL/commit/678b8a0b6a4a5be554264969fe02948bdefb92c2))
* add new classes and modules to support dependency injection ([6248a33](https://www.github.com/tgiachi/VegaRL/commit/6248a332d60e3647838d1335b273a22e747cc439))
* add new classes and properties to support items and terrain in the game map ([8f78ac6](https://www.github.com/tgiachi/VegaRL/commit/8f78ac636a15483fae0a7ad9ce047df7005d3c8d))
* add new classes for tile, tile set, and color schema entities ([6be68cd](https://www.github.com/tgiachi/VegaRL/commit/6be68cd16d4a21c4c025047dc8096a3b17712eef))
* add new directory types to DirectoryNameType enum ([063db34](https://www.github.com/tgiachi/VegaRL/commit/063db349f7d01e45966d07e86cf63d27703b15d9))
* add new entities and properties to the project ([b119362](https://www.github.com/tgiachi/VegaRL/commit/b1193625ce3993a38f5d5ebad71459e999f9734b))
* add support for default language in VegaEngineOption class ([aa302dd](https://www.github.com/tgiachi/VegaRL/commit/aa302dde724ec05e8a8cd0fc656fe73dc8d833f0))
* add support for furniture class id in FurnitureEntity to link it to a FurnitureClassEntity ([58d363d](https://www.github.com/tgiachi/VegaRL/commit/58d363dc8ff42396fa82c1e9cae94ff556f7f518))
* add support for random item generation based on a weighted probability system ([72664e8](https://www.github.com/tgiachi/VegaRL/commit/72664e8401e2c3f3722fd8b3ae12c0215397466c))
* add VegetationEntity class to represent vegetation data ([33811c5](https://www.github.com/tgiachi/VegaRL/commit/33811c5ffc66e7fd80b88463125200fdf24d7a69))
* **.editorconfig:** add .editorconfig file with rules for indentation, line length, and whitespace to ensure consistent code style across the project ([255364e](https://www.github.com/tgiachi/VegaRL/commit/255364e684257008e7861e6df235159ca25245f1))
* **BaseEntity.cs:** add HasFlag method to check if entity has a specific flag ([efbb248](https://www.github.com/tgiachi/VegaRL/commit/efbb2483914f31ba742a17632719216b08718bcf))
* **CODE_OF_CONDUCT.md:** add Contributor Covenant Code of Conduct to ensure a welcoming and harassment-free environment for all contributors and maintainers ([af05d52](https://www.github.com/tgiachi/VegaRL/commit/af05d52d0badf9cc3d1ac014c7b9049435644fcf))
* **CreatureGameObject.cs:** add Name property to CreatureGameObject class to store creature name ([63379fd](https://www.github.com/tgiachi/VegaRL/commit/63379fd14426d5037839c1ab5de9b3e75b0c9784))
* **DataService.cs:** add event messages to the message bus when loading data and when data is loaded ([e04d6f2](https://www.github.com/tgiachi/VegaRL/commit/e04d6f26cb6d2807517bfcc986fba86cb489d4ec))
* **EntityDataAttribute.cs:** add summary to EntityDataAttribute class to improve documentation ([f82ed17](https://www.github.com/tgiachi/VegaRL/commit/f82ed17885afac32879fcc0d17f52ba81f39fa19))
* **ITickService.cs, TickService.cs:** add TickService and ITickService interfaces ([d488079](https://www.github.com/tgiachi/VegaRL/commit/d488079812e4c087fed139a5c34099224cc429f1))
* **KeybindingAttribute.cs:** add KeybindingAttribute class to allow for defining keybindings for classes ([80f84a0](https://www.github.com/tgiachi/VegaRL/commit/80f84a08004aa0c4328cfc5adbfcdd662c570e00))
* **MapLayerType.cs:** add new map layer types to support new game features ([285fdbd](https://www.github.com/tgiachi/VegaRL/commit/285fdbd320c8d8645e9695d599783f036e56d556))
* **NameEntity.cs:** add new properties to NameEntity to support name types, gender types, and a list of names ([a36582a](https://www.github.com/tgiachi/VegaRL/commit/a36582a10419a7702a1e65b577b94da69d856b18))
* **TileService:** add support for loading tiles and tilesets from data files ([c34e9b0](https://www.github.com/tgiachi/VegaRL/commit/c34e9b0ea430722e61dffde6361fac83edc425e7))
* **Vega:** add support for loading and parsing JSON data files for entities ([26eb416](https://www.github.com/tgiachi/VegaRL/commit/26eb4161cb5b79ed9e4c3453ad4cd64023f306fc))
* **VegaEngineOption.cs:** add VegaUiOption property to VegaEngineOption class to store UI configuration ([ee6483e](https://www.github.com/tgiachi/VegaRL/commit/ee6483e1838f94dd693a0eea0e08966bb3218f94))

### Bug Fixes

* **README.md:** correct Venomaus' github username from Venomaus to Ven0maus to fix broken link ([0e6c25c](https://www.github.com/tgiachi/VegaRL/commit/0e6c25c3a85dfbb090e841adfc5a83eaf981e0c5))

### Other

* first commit ([f39ace5](https://www.github.com/tgiachi/VegaRL/commit/f39ace56e571ef8d246ea0b01057fb5ca7bd59fb))
* change GenderTypeEnum.Neutral to GenderTypeEnum.None for better semantics ([7c4ed5c](https://www.github.com/tgiachi/VegaRL/commit/7c4ed5c1861dfdd7000ad3659157684bdca3f55d))
* delete unused files and fix namespaces in remaining files to match new project structure ([cf1d345](https://www.github.com/tgiachi/VegaRL/commit/cf1d34555e46b3110713d718547375f1cb211cad))
* **BaseTerrainGameObject.cs:** remove unused layer parameter from BaseTerrainGameObject constructor ([c5f803c](https://www.github.com/tgiachi/VegaRL/commit/c5f803c470c7364e2667e14e244dee87deac8e98))
* **CONTRIBUTING.md:** add contribution guidelines and code of conduct ([74a8b17](https://www.github.com/tgiachi/VegaRL/commit/74a8b1788376c9691937a26305bcf8dc1d467232))
* **CreatureGroupEntity.cs:** change Creatures property type from Dictionary to RandomBagEntity to improve semantics ([ae355dc](https://www.github.com/tgiachi/VegaRL/commit/ae355dccad2cc80dd25c1ef1bf14b7b6bb1d9b04))
* **ITileService.cs:** remove unused using statement ([5ca057d](https://www.github.com/tgiachi/VegaRL/commit/5ca057da4b9fea01ee151deff6cc004fdbfa914f))
* **TerrainEntity.cs:** inherit from BaseEntity instead of IBaseEntity and remove unused properties ([de5d51f](https://www.github.com/tgiachi/VegaRL/commit/de5d51f18d1208bb5d93cf681ea70201422382b1))
* **TickService.cs:** extract regex pattern to a static field for better readability and reusability ([295dc0e](https://www.github.com/tgiachi/VegaRL/commit/295dc0e8506a2cd634d9e66708e39757bf0e24e0))
* **TickService.cs:** make _pattern and _regex fields readonly for better performance and thread safety ([4aed4e9](https://www.github.com/tgiachi/VegaRL/commit/4aed4e96f9cbcf73e10abb6065902c258c7aab56))

