PGDMP                     	    y         
   ShopBridge    12.4    12.4 
               0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            	           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            
           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false                       1262    91307 
   ShopBridge    DATABASE     ?   CREATE DATABASE "ShopBridge" WITH TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'English_India.1252' LC_CTYPE = 'English_India.1252';
    DROP DATABASE "ShopBridge";
                postgres    false            ?            1259    91313    Product    TABLE     ?   CREATE TABLE public."Product" (
    "Id" uuid NOT NULL,
    "Name" text,
    "Description" text,
    "Price" integer NOT NULL
);
    DROP TABLE public."Product";
       public         heap    postgres    false            ?            1259    91308    __EFMigrationsHistory    TABLE     ?   CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);
 +   DROP TABLE public."__EFMigrationsHistory";
       public         heap    postgres    false                      0    91313    Product 
   TABLE DATA           I   COPY public."Product" ("Id", "Name", "Description", "Price") FROM stdin;
    public          postgres    false    203   K
                 0    91308    __EFMigrationsHistory 
   TABLE DATA           R   COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
    public          postgres    false    202   h
       ?
           2606    91320    Product PK_Product 
   CONSTRAINT     V   ALTER TABLE ONLY public."Product"
    ADD CONSTRAINT "PK_Product" PRIMARY KEY ("Id");
 @   ALTER TABLE ONLY public."Product" DROP CONSTRAINT "PK_Product";
       public            postgres    false    203            ?
           2606    91312 .   __EFMigrationsHistory PK___EFMigrationsHistory 
   CONSTRAINT     {   ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");
 \   ALTER TABLE ONLY public."__EFMigrationsHistory" DROP CONSTRAINT "PK___EFMigrationsHistory";
       public            postgres    false    202                  x?????? ? ?         0   x?32024400147106???/p*?LIO?5?4?3?34?????? Ȧ	?     