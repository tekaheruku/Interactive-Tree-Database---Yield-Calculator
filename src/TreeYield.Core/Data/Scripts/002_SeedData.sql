-- =====================================================================
-- 002_SeedData.sql - categories and sample tree species.
--
-- IMPORTANT: yields and prices are illustrative planning estimates for a
-- school demonstration. Real yields depend on variety, age, spacing, soil
-- and care, and farm-gate prices change with season and market. Verify
-- with the Department of Agriculture (DA), the Philippine Statistics
-- Authority (PSA) or local buyers before making real decisions.
--
-- Units: YieldPerTreePerYear is per mature tree per year.
--   Annual species   = yearly harvest at full production.
--   Rotation species = mean annual increment in board feet; the trees
--                      are cut once at rotation age (YearsToFirstHarvest).
--
-- Every INSERT skips rows that already exist (WHERE NOT EXISTS), so this
-- script can be run again safely, for example by "Reset Sample Data".
-- =====================================================================

INSERT INTO dbo.Categories (Name, Description)
SELECT v.Name, v.Description
FROM (VALUES
    (N'Fruit-Bearing',     N'Trees grown mainly for fruit, nuts or beans that are harvested every year.'),
    (N'Non-Fruit-Bearing', N'Trees grown mainly for timber, pulpwood or latex instead of fruit.')
) AS v (Name, Description)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Categories AS c WHERE c.Name = v.Name);
GO

INSERT INTO dbo.TreeSpecies
    (CommonName, ScientificName, CategoryId, ProductType, YieldUnit, YieldPerTreePerYear, PricePerUnit,
     HarvestType, YearsToFirstHarvest, YearsToFullProduction, HarvestSeason, Notes)
SELECT v.CommonName, v.ScientificName, c.CategoryId, v.ProductType, v.YieldUnit, v.YieldPerTree, v.Price,
       v.HarvestType, v.FirstHarvest, v.FullProduction, v.Season, v.Notes
FROM (VALUES
    -- Fruit-bearing trees (harvested every year)
    (N'Carabao Mango', N'Mangifera indica ''Carabao''', N'Fruit-Bearing', N'Fresh fruit', N'kg', 300.00, 80.00, N'Annual', 5, 10, N'March - June',
     N'The Philippine "sweetest mango". Grafted trees bear earlier; flower induction improves yield. Sample estimate.'),
    (N'Coconut', N'Cocos nucifera', N'Fruit-Bearing', N'Mature nuts', N'pcs', 60.00, 15.00, N'Annual', 6, 10, N'Year-round (every 45-60 days)',
     N'Yield counted in whole nuts. Copra, coco water and husk add income not included here. Sample estimate.'),
    (N'Calamansi', N'Citrus x microcarpa', N'Fruit-Bearing', N'Fresh fruit', N'kg', 20.00, 60.00, N'Annual', 2, 5, N'August - January',
     N'Fast to bear and suited to backyards. Prices rise sharply off-season. Sample estimate.'),
    (N'Rambutan', N'Nephelium lappaceum', N'Fruit-Bearing', N'Fresh fruit', N'kg', 80.00, 70.00, N'Annual', 5, 9, N'August - October',
     N'Needs deep, well-drained soil and regular rainfall. Sample estimate.'),
    (N'Lanzones', N'Lansium parasiticum', N'Fruit-Bearing', N'Fresh fruit', N'kg', 60.00, 90.00, N'Annual', 8, 15, N'September - November',
     N'Slow to bear from seed; Camiguin and Laguna are known producers. Sample estimate.'),
    (N'Durian', N'Durio zibethinus', N'Fruit-Bearing', N'Fresh fruit', N'kg', 120.00, 100.00, N'Annual', 6, 12, N'August - October',
     N'Davao is the main producing region. Grafted varieties command higher prices. Sample estimate.'),
    (N'Jackfruit (Langka)', N'Artocarpus heterophyllus', N'Fruit-Bearing', N'Fresh fruit', N'kg', 250.00, 25.00, N'Annual', 3, 7, N'Year-round (peak March - August)',
     N'Very heavy fruits; unripe fruit is also sold as a vegetable. Sample estimate.'),
    (N'Avocado', N'Persea americana', N'Fruit-Bearing', N'Fresh fruit', N'kg', 100.00, 60.00, N'Annual', 4, 8, N'June - September',
     N'Grafted trees give more uniform fruit. Sample estimate.'),
    (N'Cacao', N'Theobroma cacao', N'Fruit-Bearing', N'Dried beans', N'kg', 1.50, 180.00, N'Annual', 3, 6, N'Year-round (peak October - January)',
     N'Yield is fermented and dried beans. Often intercropped under coconut. Sample estimate.'),
    (N'Robusta Coffee', N'Coffea canephora', N'Fruit-Bearing', N'Green coffee beans', N'kg', 1.00, 120.00, N'Annual', 3, 5, N'November - February',
     N'Yield is green (unroasted) beans per tree. Sample estimate.'),
    (N'Guyabano', N'Annona muricata', N'Fruit-Bearing', N'Fresh fruit', N'kg', 40.00, 80.00, N'Annual', 3, 6, N'Year-round',
     N'Soursop. Hand pollination can raise fruit set. Sample estimate.'),
    (N'Pomelo (Suha)', N'Citrus maxima', N'Fruit-Bearing', N'Fresh fruit', N'kg', 100.00, 50.00, N'Annual', 5, 8, N'August - October',
     N'Large citrus; Davao varieties are well known. Sample estimate.'),
    (N'Santol', N'Sandoricum koetjape', N'Fruit-Bearing', N'Fresh fruit', N'kg', 100.00, 30.00, N'Annual', 5, 10, N'June - August',
     N'Hardy tree that tolerates poor soils. Sample estimate.'),
    (N'Pili', N'Canarium ovatum', N'Fruit-Bearing', N'Nuts in shell', N'kg', 30.00, 50.00, N'Annual', 6, 12, N'May - October',
     N'Native to the Bicol region. Shelled kernels sell for much more. Sample estimate.'),
    (N'Mangosteen', N'Garcinia mangostana', N'Fruit-Bearing', N'Fresh fruit', N'kg', 50.00, 150.00, N'Annual', 8, 15, N'July - September',
     N'"Queen of fruits". Very slow to bear but high value. Sample estimate.'),

    -- Non-fruit-bearing trees (timber: cut once at rotation age; rubber: tapped every year)
    (N'Mahogany', N'Swietenia macrophylla', N'Non-Fruit-Bearing', N'Timber', N'board ft', 8.00, 70.00, N'Rotation', 25, 25, N'At rotation age',
     N'Yield is mean annual increment in board feet; the tree is cut once at rotation age. Sample estimate.'),
    (N'Gmelina (Yemane)', N'Gmelina arborea', N'Non-Fruit-Bearing', N'Timber', N'board ft', 15.00, 35.00, N'Rotation', 10, 10, N'At rotation age',
     N'Fast-growing plantation species for lumber and furniture. Sample estimate.'),
    (N'Falcata', N'Falcataria falcata', N'Non-Fruit-Bearing', N'Peeler logs / timber', N'board ft', 20.00, 25.00, N'Rotation', 8, 8, N'At rotation age',
     N'Very fast-growing; sold to plywood and veneer mills in Mindanao. Sample estimate.'),
    (N'Teak', N'Tectona grandis', N'Non-Fruit-Bearing', N'Timber', N'board ft', 6.00, 120.00, N'Rotation', 20, 20, N'At rotation age',
     N'Premium, durable hardwood. Long rotation but high price. Sample estimate.'),
    (N'Acacia Mangium', N'Acacia mangium', N'Non-Fruit-Bearing', N'Pulpwood / timber', N'board ft', 15.00, 25.00, N'Rotation', 8, 8, N'At rotation age',
     N'Fast-growing; improves poor soils by fixing nitrogen. Sample estimate.'),
    (N'Bagras (Rainbow Eucalyptus)', N'Eucalyptus deglupta', N'Non-Fruit-Bearing', N'Timber / pulpwood', N'board ft', 12.00, 30.00, N'Rotation', 12, 12, N'At rotation age',
     N'Native to Mindanao; used for pulp, plywood and light construction. Sample estimate.'),
    (N'Rubber', N'Hevea brasiliensis', N'Non-Fruit-Bearing', N'Cup lump (latex)', N'kg', 4.00, 35.00, N'Annual', 6, 9, N'Year-round tapping (rest when leaves fall)',
     N'Tapped for latex every year once the trunk is thick enough. Sample estimate.')
) AS v (CommonName, ScientificName, CategoryName, ProductType, YieldUnit, YieldPerTree, Price,
        HarvestType, FirstHarvest, FullProduction, Season, Notes)
INNER JOIN dbo.Categories AS c ON c.Name = v.CategoryName
WHERE NOT EXISTS (SELECT 1 FROM dbo.TreeSpecies AS t WHERE t.CommonName = v.CommonName);
GO
