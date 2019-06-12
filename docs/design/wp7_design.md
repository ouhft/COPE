# Design - WP7:Biobanking

Design and development notes

## Initial interviews
Talking with Bhumika on 1st Feb 2016

This is a consolidation of all the samples across all the sites, with Oxford as the central repository. Eventually all the samples have to end up back here. Part of the process is physically tracking where they are. The aim is to track the entire lifespan of the sample, from collection, through to depletion.

On recipt at Oxford, there's processing including aliquoting (breaking it up into smaller samples). Start with a specimen (e.g. a liver biopsy), and then process it into different forms.

The people using this are WP7 people:

- Bhumika and Rajeev as admins
- "Allocated people" e.g. Sarah and Sandra in Essen; Tim and Ron in Gronigen; Steffie and Sander in Maastricht; Mihai in Barcelona;

These users are based at central storage sites, where they have their own freezers.


### The sample types and their journeys

See white board images

#### Biopsies

Types: 

* Frozen (LN2)
* RNAlater (solution for preserving a biopsy)
* Formalin

Frozen biopsies come from half of a sample taken with a biopsy gun (the other half goes into the other two types), and put into a cryovial. Stored at -80degC, and then transferred to Oxford

RNAlater for the LT1 samples (only in WP2) is collected (with the TT disposing of the fluid), and then having the sample snap frozen in dry ice. The separation often happens hours before the logging of the sample (which occurs at another site). This only applies to cold stored samples.

Note to self: Get pathway diagrams drawn of all this


#### Perfusate

One type, from different organs, e.g. kidney or liver

In WP2, they're collected in Blood Vacutainer (aka the blood collection tubes), and processed similar to blood

In WP3 and WP4, collected from the device.

#### Blood

Preparation types: 

* Purple top Vacutainer, EDTA (ref?) - its an additive to stop the blood clotting, which produces Plasma (with cells)
* Gold top Vacutainer, SST - its an additive to allow the blood to clot and has a separator gel, which produces Serum (without cells)

#### Urine

As it sounds like. Gets centrifuged to remove the cells.

### General

Needs to have an audit trail for HTAuthority compliance.

There's a barcode scanner to investigate for collecting the Barcode ID. Currently scans into Excel and then C/P into DB.

Locations are: Position and Box

WP7 currently pulls a list of barcodes from the WP2 DB, and the ones that aren't scanned in appear in a separate list for reference. Could be exported to an XLSX and imported

* Accessesioned (logging / accepting of samples)
* Store
* Aliquoted
* Allocated

