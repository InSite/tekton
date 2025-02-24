-- Load partitions 1-7 and 99.

INSERT INTO security.t_partition 
 (partition_number, partition_slug, partition_host, partition_name, partition_email, modified_when, partition_testers) 
VALUES
 ( 1, 'E01', 'e01.shiftiq.com', 'Demo'             , 'support@shiftiq.com', now(), 'girl.feldy@gmail.com')
,( 2, 'E02', 'e02.insite.com' , 'General'          , 'support@shiftiq.com', now(), 'sandra@bcpvpa.bc.ca,david@bcpvpa.bc.ca')
,( 3, 'E03', 'e03.insite.com' , 'CMDS'             , 'support@shiftiq.com', now(), 'girl.feldy@gmail.com')
,( 4, 'E04', 'e04.shiftiq.com', 'Skilled Trades BC', 'support@shiftiq.com', now(), 'girl.feldy@gmail.com')
,( 5, 'E05', 'e05.shiftiq.com', 'Demo'             , 'support@shiftiq.com', now(), 'girl.feldy@gmail.com')
,( 6, 'E06', 'e06.insite.com' , 'Inspire'          , 'support@shiftiq.com', now(), 'girl.feldy@gmail.com')
,( 7, 'E07', 'e07.shiftiq.com', 'SkillsCheck'      , 'support@shiftiq.com', now(), 'girl.feldy@gmail.com')
,(99, 'E99', 'e99.shiftiq.com', 'Test'             , 'support@shiftiq.com', now(), 'girl.feldy@gmail.com')
;

-- Load entities

insert into metadata.t_entity (component_type,component_name,component_feature,entity_id,entity_name,collection_slug,collection_key,storage_structure,storage_schema,storage_table,storage_key) 
values
  ('Utility','Bus','Tracking','9ec19f6c-be6d-4932-a34a-b8694ff2ee76','Aggregate','aggregates','{aggregate:guid}','Table','bus','t_aggregate','aggregate_id')
, ('Utility','Bus','Tracking','b2298c33-a0ff-4de7-be91-5272306426fb','Event','events','{event:guid}','Table','bus','t_event','event_id')
, ('Application','Content','Text','dcc37f11-65e9-47b3-a271-64faa9047400','Translation','translations','{translation:guid}','Table','content','t_translation','translation_id')
, ('Application','Contact','Location','a76eb82e-ad02-4341-b4eb-386710b4e651','Country','countrys','{country:guid}','Table','location','t_country','country_id')
, ('Application','Contact','Location','99e52dba-1c6a-4543-a5f4-1130a3df0748','Province','provinces','{province:guid}','Table','location','t_province','province_id')
, ('Utility','Metadata','Audit','71863ff2-ff87-496e-ac3e-239bf8959d12','Origin','origins','{origin:guid}','Table','metadata','t_origin','origin_id')
, ('Utility','Metadata','Audit','146c1238-8f91-4600-ace8-eaa4e630b527','Version','versions','{version:guid}','Table','metadata','t_version','version_number')
, ('Utility','Security','Identification','1391ff37-6653-4751-82c7-a34ed6e294e6','Organization','organizations','{organization:guid}','Table','security','t_organization','organization_id')
, ('Utility','Security','Identification','86688277-8420-470d-8380-f9cbbbd3d82d','Partition','partitions','{partition:guid}','Table','security','t_partition','partition_number')
, ('Utility','Security','Identification','f3dd3240-987e-4338-862f-f620954fba60','Password','passwords','{password:guid}','Table','security','t_password','password_id')
, ('Utility','Security','Authorization','2316b570-5ff0-4740-99db-ffb83ce58660','Permission','permissions','{permission:guid}','Table','security','t_permission','permission_id')
, ('Utility','Security','Authorization','aae8bd46-18cd-476b-a112-42b9a9946933','Resource','resources','{resource:guid}','Table','security','t_resource','resource_id')
, ('Utility','Security','Identification','07791095-2831-4f2f-8661-0cd329e85fab','Role','roles','{role:guid}','Table','security','t_role','role_id')
;